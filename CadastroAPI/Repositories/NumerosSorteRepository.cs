using CadastroAPI.Models;
using Microsoft.Data.SqlClient;
using CadastroAPI.Context;
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

        public IEnumerable<NumerosSorte> GerarNumerosSorte(string idUsuario, string idNotaFiscal, int quantidade)
        {
            var numerosSorte = new List<NumerosSorte>();

            var idUsuarioParam = new SqlParameter("@IdUsuario", idUsuario);
            var idNotaFiscalParam = new SqlParameter("@IdNotaFiscal", idNotaFiscal);
            var quantidadeParam = new SqlParameter("@Quantidade", quantidade);

            numerosSorte = _context.NumerosSorte.FromSqlRaw("EXEC GerarNumerosAleatorios @IdUsuario, @IdNotaFiscal, @Quantidade", idUsuarioParam, idNotaFiscalParam, quantidadeParam).ToList();

            return numerosSorte;
        }
    }

}
