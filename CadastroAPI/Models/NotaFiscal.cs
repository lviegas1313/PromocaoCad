using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroAPI.Models
{
    public class NotaFiscal
    {

        [Key, Column(Order = 0)]
        [MaxLength(11)]
        public string UsuarioId { get; set; } // Chave primária junto com NotaCupom

        [Key, Column(Order = 1)]
        public string NotaCupom { get; set; } // Chave primária junto com UsuarioId

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }

        [Required]
        public List<Produto>? Produtos { get; set; }

        public Imagem Imagem { get; set; } // Referência para a imagem

        public NotaFiscal(string notaCupom, string cnpj, DateTime dataCompra)
        {
            NotaCupom = notaCupom;
            Cnpj = cnpj;
            DataCompra = dataCompra;
        }
        public NotaFiscal() { }
    }
    public class Produto
    {
        [Key]
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
        public Produto(string nome, string versao)
        {
            Nome = nome;
            Versao = versao;
        }
    }
    public class Imagem
    {
        [Key]
        public string NotaFiscalId { get; set; } // Chave estrangeira para a nota fiscal
        [Required]
        public byte[] Dados { get; set; } // Dados da imagem para armazenamento

        [NotMapped]
        public MemoryStream ImagemStream
        {
            get => new MemoryStream(Dados);
            set
            {
                using (var memoryStream = new MemoryStream())
                {
                    value.CopyTo(memoryStream);
                    Dados = memoryStream.ToArray();
                }
            }
        }
        public Imagem() { }
        public Imagem(string notaFiscalId, IFormFile imagem)
        {
            NotaFiscalId = notaFiscalId;
            SetImagemFromIFormFile(imagem);
        }

        public void SetImagemFromIFormFile(IFormFile imagem)
        {
            using (var stream = new MemoryStream())
            {
                imagem.CopyTo(stream);
                ImagemStream = stream;
            }
        }
    }
    public class NumerosSorte()
    {
        [Required]
        public string NotaFiscalId { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        [Key]
        public string Numero { get; set; }
        public DateOnly DataSorteio { get; set; }
        public DateTime DataCadastro { get; set; }        
    }
}