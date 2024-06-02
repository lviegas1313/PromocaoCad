using CadastroAPI.Context;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace CadastroAPI.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly CadastroContext _context;

        public DatabaseRepository(CadastroContext context)
        {
            _context = context;
        }

        public async Task ApplyMigrationsAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync();
            }
        }

        public async Task EnsureProcedureExistsAsync(string procedureSql)
        {
            var procedureName = GetProcedureNameFromSql(procedureSql);

            var checkProcedureExists = $@"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = '{procedureName}')
                BEGIN
                    EXEC sp_executesql N'{procedureSql.Replace("'", "''")}'
                END
            ";

            await _context.Database.ExecuteSqlRawAsync(checkProcedureExists);
        }

        public async Task EnsureProcedureExistsFromFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                var procedureSql = await File.ReadAllTextAsync(filePath);
                var procedureName = GetProcedureNameFromSql(procedureSql);

                // Consultar se a procedure já existe no banco de dados
                var existingProcedureSql = await GetExistingProcedureAsync(procedureName);

                if (existingProcedureSql != null && existingProcedureSql != procedureSql)
                {
                    // Se a procedure existir e o código for diferente, alterar a procedure
                    var alterProcedureSql = $"ALTER PROCEDURE {procedureName} AS BEGIN {procedureSql} END";
                    await _context.Database.ExecuteSqlRawAsync(alterProcedureSql);
                }
                else if (existingProcedureSql == null)
                {
                    // Se a procedure não existir, criar a procedure
                    await EnsureProcedureExistsAsync(procedureSql);
                }
                else
                {
                    Console.WriteLine("A procedure já está atualizada.");
                }
            }
            else
            {
                Console.WriteLine($"O arquivo SQL '{filePath}' não foi encontrado.");
            }
        }
        public async Task TruncateTablesAsync(List<string> tableNames)
        {
#if DEBUG
            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            foreach (var tableName in tableNames)
            {
                try
                {
                    var truncateCommand = $"TRUNCATE TABLE {tableName}";
                    await _context.Database.ExecuteSqlRawAsync(truncateCommand);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao truncar tabela {tableName}: {ex.Message}");
                }
            }

            // Fechar a conexão
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync();
            }
#else
    Console.WriteLine("O truncamento das tabelas só é permitido em ambiente de desenvolvimento.");
#endif
        }

        private async Task<string> GetExistingProcedureAsync(string procedureName)
        {
            // Consultar o código SQL da procedure existente no banco de dados
            var sqlQuery = $@"SELECT OBJECT_DEFINITION(OBJECT_ID('{procedureName}')) AS ProcedureSql";

            var procedureDefinition = await _context.Database.ExecuteSqlRawAsync(sqlQuery);

            return procedureDefinition.ToString();
        }


        private string GetProcedureNameFromSql(string procedureSql)
        {
            // Extrair o nome da procedure do código SQL
            var match = Regex.Match(procedureSql, @"CREATE\s+PROCEDURE\s+(\[?\w+\]?\.)?\[?(\w+)\]?");
            if (match.Success && match.Groups.Count >= 3)
            {
                return match.Groups[2].Value;
            }

            throw new ArgumentException("O código SQL não contém uma definição válida de procedure.");
        }
    }
}
