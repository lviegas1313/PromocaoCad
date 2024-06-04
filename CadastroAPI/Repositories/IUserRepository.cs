using CadastroAPI.Models;

namespace CadastroAPI.Repositories
{
    public interface IUserRepository
    {
        Task<Usuario> GetByIdAsync(string CPF);
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> AddAsync(Usuario user);
        Task<Usuario> UpdateAsync(Usuario user);
        Task DeleteAsync(Usuario user);
    }
}
