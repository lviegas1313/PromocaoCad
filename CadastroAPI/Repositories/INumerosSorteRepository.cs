using CadastroAPI.Models;

namespace CadastroAPI.Repositories
{
    public interface INumerosSorteRepository
    {
        IEnumerable<NumerosSorte> GerarNumerosSorte(string idUsuario, string idNotaFiscal, int quantidade);
    }

}
