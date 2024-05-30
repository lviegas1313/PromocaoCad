using Microsoft.EntityFrameworkCore;

namespace CadastroAPI.Models
{

    public class CadastroContext : DbContext
    {
        public CadastroContext(DbContextOptions<CadastroContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

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
                entity.Property(e => e.Imagem).HasColumnType("varbinary(max)");// Configurar a coluna como varbinary(max) no SQL Server
            });

        }

        public DbSet<NotaFiscal> NotasFiscais { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
