﻿using CadastroAPI.Context;
using CadastroAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CadastroAPI.Repositories
{
    public class NumerosSorteRepository : INumerosSorteRepository
    {
        private readonly CadastroContext _context;

        public NumerosSorteRepository(CadastroContext context)
        {
            _context = context;
        }

        public IEnumerable<NumeroSorte> GerarNumerosSorte(string idUsuario, string idNotaFiscal, int quantidade)
        {
            var numerosSorte = new List<NumeroSorte>();

            var idUsuarioParam = new SqlParameter("@IdUsuario", idUsuario);
            var idNotaFiscalParam = new SqlParameter("@IdNotaFiscal", idNotaFiscal);
            var quantidadeParam = new SqlParameter("@Quantidade", quantidade);

            numerosSorte = _context.NumerosSorte.FromSqlRaw("EXEC GerarNumerosAleatorios @IdUsuario, @IdNotaFiscal, @Quantidade", idUsuarioParam, idNotaFiscalParam, quantidadeParam).ToList();

            return numerosSorte;
        }
        public async Task<IEnumerable<NumeroSorte>> GerarNumerosSorteAsync(string idUsuario, string idNotaFiscal, int quantidade,DateTime? dataSorteio = null)
        {
            DateTime dataSorteioFixa = dataSorteio ?? new DateTime(2000, 1, 1);
            var idUsuarioParam = new SqlParameter("@IdUsuario", idUsuario);
            var idNotaFiscalParam = new SqlParameter("@IdNotaFiscal", idNotaFiscal);
            var quantidadeParam = new SqlParameter("@Quantidade", quantidade);
            var dataSorteioParam = new SqlParameter("@DataSorteio", dataSorteioFixa);


            // Utilize o método FromSqlRawAsync para operações assíncronas
            var numerosSorte = await _context.NumerosSorte.FromSqlRaw("EXEC GerarNumerosAleatorios @IdUsuario, @IdNotaFiscal, @Quantidade,@Datasorteio", idUsuarioParam, idNotaFiscalParam, quantidadeParam, dataSorteioParam).ToListAsync();

            return numerosSorte;
        }

        public async Task<List<NumeroSorteDTO>> GetNumerosPorNotaFiscal(string notaFiscalId)
        {
            var numerosSorte = await _context.NumerosSorte
                .Where(n => n.NotaFiscalId == notaFiscalId)
                .Select(n => new NumeroSorteDTO
                {
                    Numero = n.Numero,
                    DataSorteio = n.DataSorteio
                })
                .ToListAsync();

            return numerosSorte;
        }

        public async Task<List<NumeroSorteDTO>> GetNumerosPorUsuario(string usuarioId)
        {
            var numerosSorte = await _context.NumerosSorte
                .Where(n => n.UsuarioId == usuarioId)
                .Select(n => new NumeroSorteDTO
                {
                    Numero = n.Numero,
                    DataSorteio = n.DataSorteio
                })
                .ToListAsync();

            return numerosSorte;
        }

    }

}
