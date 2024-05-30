using System.ComponentModel.DataAnnotations;

namespace CadastroAPI.Models
{
    public class NotaFiscal
    {
        [Key]
        [Required]
        public string NotaCupom { get; set; }

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }

        [Required]
        public List<Produto>? Produtos { get; set; }

        public MemoryStream? Imagem { get; set; }

        public NotaFiscal(string notaCupom, string cnpj, DateTime dataCompra)
        {
            NotaCupom = notaCupom;
            Cnpj = cnpj;
            DataCompra = dataCompra;            
        }
    }
    public class Produto
    {
        [Required]
        public string Nome { get; set; }
        public string Versao { get; set; }
        [Required]
        public int Quantidade { get; set; }
        [Required]
        public decimal Valor {  get; set; }

        public Produto(string nome, string versao)
        {
            Nome = nome;
            Versao = versao;
        }
    }
}
