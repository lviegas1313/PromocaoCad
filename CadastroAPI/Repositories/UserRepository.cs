using CadastroAPI.Context;
using CadastroAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CadastroContext _context;

        public UserRepository(CadastroContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(string CPF)
        {
            return await _context.Users.FindAsync(CPF);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}
