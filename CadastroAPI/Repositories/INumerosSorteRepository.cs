using CadastroAPI.Models;

namespace CadastroAPI.Repositories
{
    public interface INumerosSorteRepository
    {
        IEnumerable<NumeroSorte> GerarNumerosSorte(string idUsuario, string idNotaFiscal, int quantidade);
        Task<IEnumerable<NumeroSorte>> GerarNumerosSorteAsync(string idUsuario, string idNotaFiscal, int quantidade);
        Task<List<NumeroSorteDTO>> GetNumerosPorNotaFiscal(string notaFiscalId);
        Task<List<NumeroSorteDTO>> GetNumerosPorUsuario(string usuarioId);
    }

}
