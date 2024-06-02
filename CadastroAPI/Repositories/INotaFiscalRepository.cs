using CadastroAPI.Models;

namespace CadastroAPI.Repositories
{
    public interface INotaFiscalRepository
    {
        Task AddNotaFiscalAsync(NotaFiscal notaFiscal);
        Task AddNotaFiscalAndImagemAsync(NotaFiscal notaFiscal, Imagem imagem);
        Task<NotaFiscal> GetNotaFiscalAsync(string usuarioId, string notaCupom);
        Task<IEnumerable<NotaFiscal>> GetNotasFiscaisByUsuarioIdAsync(string usuarioId);
    }

}
