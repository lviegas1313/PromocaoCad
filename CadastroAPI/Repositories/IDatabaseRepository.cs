namespace CadastroAPI.Repositories
{
    public interface IDatabaseRepository
    {
        Task ApplyMigrationsAsync();
        Task EnsureProcedureExistsAsync(string procedureSql);
        Task EnsureProcedureExistsFromFileAsync(string filePath);
        Task TruncateTablesAsync(List<string> tableNames);
    }
}
