using CadastroAPI.Models;

namespace CadastroAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string CPF);
        Task<User> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
