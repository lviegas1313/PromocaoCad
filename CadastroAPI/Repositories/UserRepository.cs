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

        public async Task<Usuario> GetByIdAsync(string cpf)
        {
            var user = await _context.Users.FindAsync(cpf);
            if (user == null)
            {  
                throw new HttpRequestException($"User with CPF {cpf} not found.");
            }
            return user;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {               
                throw new HttpRequestException($"User with email {email} not found.");
            }
            return user;
        }

        public async Task<Usuario> AddAsync(Usuario user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Usuario> UpdateAsync(Usuario user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(Usuario user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

}
