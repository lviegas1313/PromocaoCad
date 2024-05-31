using CadastroAPI.Context;
using CadastroAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroAPI.Repositories
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly CadastroContext _context;

        public NotaFiscalRepository(CadastroContext context)
        {
            _context = context;
        }

        public async Task AddNotaFiscalAndImagemAsync(NotaFiscal notaFiscal, Imagem imagem)
        {
            await _context.NotasFiscais.AddAsync(notaFiscal);
            await _context.Imagens.AddAsync(imagem);
            await _context.SaveChangesAsync();
        }
        public async Task<NotaFiscal> GetNotaFiscalAsync(string usuarioId, string notaCupom)
        {
            var notaFiscal = await _context.NotasFiscais
            .Include(n => n.Produtos)
            .Include(n => n.Imagem)
            .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId && n.NotaCupom == notaCupom);

            return notaFiscal ?? new NotaFiscal(); // Retorna uma nota fiscal vazia se não encontrar
        }

        public async Task<IEnumerable<NotaFiscal>> GetNotasFiscaisByUsuarioIdAsync(string usuarioId)
        {
            var notasFiscais = await _context.NotasFiscais
            .Include(n => n.Produtos)
            .Include(n => n.Imagem)
            .Where(n => n.UsuarioId == usuarioId)
            .ToListAsync();

            return notasFiscais.Count == 0 ? new List<NotaFiscal>() : notasFiscais; // Retorna uma lista vazia se não houver resultados
        }

    }
}
