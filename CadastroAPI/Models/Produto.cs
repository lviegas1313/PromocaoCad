using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroAPI.Models
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string NotaFiscalId { get; set; } // Chave estrangeira para a nota fiscal

        [Required]
        public string Nome { get; set; }

        public string Versao { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal Valor { get; set; }

        public Produto() { }

        public static Produto FromDto(ProdutoDTO dto, string notaFiscalId)
        {
            return new Produto
            {
                NotaFiscalId = notaFiscalId,
                Nome = dto.Nome,
                Versao = dto.Versao,
                Quantidade = dto.Quantidade,
                Valor = dto.Valor
            };
        }

        public ProdutoDTO ToDto()
        {
            return new ProdutoDTO
            {
                Nome = this.Nome,
                Versao = this.Versao,
                Quantidade = this.Quantidade,
                Valor = this.Valor
            };
        }
    }

    public class ProdutoDTO
    {
        [Required]
        public string Nome { get; set; }

        public string Versao { get; set; }

        [Required]
        public int Quantidade { get; set; }

        [Required]
        public decimal Valor { get; set; }
    }
}
