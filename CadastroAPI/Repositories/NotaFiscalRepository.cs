﻿using CadastroAPI.Context;
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

        public async Task AddNotaFiscalAsync(NotaFiscal notaFiscal)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.NotasFiscais.AddAsync(notaFiscal);
                    if (notaFiscal.Produtos != null && notaFiscal.Produtos.Any())
                    {
                        await _context.Produtos.AddRangeAsync(notaFiscal.Produtos);
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task AddNotaFiscalAndImagemAsync(NotaFiscal notaFiscal, Imagem imagem)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _context.NotasFiscais.AddAsync(notaFiscal);
                    if (notaFiscal.Produtos != null && notaFiscal.Produtos.Any())
                    {
                        await _context.Produtos.AddRangeAsync(notaFiscal.Produtos);
                    }
                    await _context.Imagens.AddAsync(imagem);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<NotaFiscal> GetNotaFiscalAsync(string usuarioId, string notaCupom)
        {
            var notaFiscal = await _context.NotasFiscais
                .Include(n => n.Produtos)
                //.Include(n => n.Imagem)
                .FirstOrDefaultAsync(n => n.UsuarioId == usuarioId && n.NotaCupom == notaCupom);

            return notaFiscal ?? new NotaFiscal(); // Retorna uma nota fiscal vazia se não encontrar
        }

        public async Task<IEnumerable<NotaFiscal>> GetNotasFiscaisByUsuarioIdAsync(string usuarioId)
        {
            var notasFiscais = await _context.NotasFiscais
                .Include(n => n.Produtos)
                //.Include(n => n.Imagem)
                .Where(n => n.UsuarioId == usuarioId)
                .ToListAsync();

            return notasFiscais.Count == 0 ? new List<NotaFiscal>() : notasFiscais; // Retorna uma lista vazia se não houver resultados
        }
    }
}
