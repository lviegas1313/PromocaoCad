using CadastroAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CadastroAPI.Context
{
    public class CadastroContext : DbContext
    {
        public CadastroContext(DbContextOptions<CadastroContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public DbSet<NumerosSorte> NumerosSorte { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.CPF); // Chave primária
                entity.HasIndex(e => e.Email).IsUnique(); // Índice na coluna Email
            });

            modelBuilder.Entity<NotaFiscal>(entity =>
            {
                entity.HasKey(e => e.NotaCupom); // Chave primária
                entity.HasIndex(e => e.Cnpj); // Índice de pesquisa
            });

            modelBuilder.Entity<Imagem>(entity =>
            {
                entity.HasKey(e => e.NotaFiscalId); // Definindo a chave primária para a entidade Imagem
                entity.Property(e => e.Dados).IsRequired(); // Propriedade Dados é obrigatória
            });

            modelBuilder.Entity<Produto>(entity =>
            {
                entity.Property(e => e.Nome).IsRequired(); // Propriedade Nome é obrigatória
                entity.Property(e => e.Quantidade).IsRequired(); // Propriedade Quantidade é obrigatória
                entity.Property(e => e.Valor).IsRequired().HasColumnType("decimal(18, 2)"); // Propriedade Valor é obrigatória
            });

            modelBuilder.Entity<NumerosSorte>(entity =>
            {
                entity.HasKey(e => e.Numero); // Definindo a chave primária para a entidade NumerosSorte
                entity.Property(e => e.NotaFiscalId).IsRequired(); // Propriedade NotaFiscalId é obrigatória
                entity.Property(e => e.UsuarioId).IsRequired(); // Propriedade UsuarioId é obrigatória
                entity.Property(e => e.DataCadastro).IsRequired(); // Propriedade DataCadastro é obrigatória
            });
        }

        // Método para configurar a imagem a partir de um IFormFile
        public Imagem ConfigureImagemFromIFormFile(string notaFiscalId, IFormFile imagem)
        {
            using (var stream = new MemoryStream())
            {
                imagem.CopyTo(stream);
                var dados = stream.ToArray();
                return new Imagem { NotaFiscalId = notaFiscalId, Dados = dados };
            }
        }

        public IEnumerable<NumerosSorte> GerarNumerosSorte(string idUsuario, string idNotaFiscal, int quantidade)
        {
            var numerosSorte = new List<NumerosSorte>();

            var idUsuarioParam = new SqlParameter("@IdUsuario", idUsuario);
            var idNotaFiscalParam = new SqlParameter("@IdNotaFiscal", idNotaFiscal);
            var quantidadeParam = new SqlParameter("@Quantidade", quantidade);

            numerosSorte = NumerosSorte.FromSqlRaw("EXEC GerarNumerosAleatorios @IdUsuario, @IdNotaFiscal, @Quantidade", idUsuarioParam, idNotaFiscalParam, quantidadeParam).ToList();

            return numerosSorte;
        }
    }
}
