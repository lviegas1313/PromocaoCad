using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CadastroAPI.Models
{
    public class NotaFiscal
    {
        const string prodCoringa = "fermento";

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
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        [JsonIgnore]
        public List<NumeroSorteDTO>? Numeros { get; set; }

        public NotaFiscal() { }

        public int CalcularNumerosSorte()
        {
            if (Produtos == null || Produtos.Sum(p => p.Quantidade) < 2)
                return 0;

            // Contagem total de produtos participantes e de fermento
            int produtosParticipantes = Produtos.Sum(p => p.Quantidade);
            int quantidadeFermento = Produtos.Where(p => p.Nome.ToLower() == prodCoringa).Sum(p => p.Quantidade);

            // Cálculo do número de sorte
            int numerosSorte = produtosParticipantes / 2; // 1 número de sorte a cada 2 produtos participantes

            // Regra do fermento
            if (produtosParticipantes % 2 != 0 && quantidadeFermento > 0)
            {
                numerosSorte++; // Adiciona mais 1 número de sorte se a quantidade de produtos for ímpar e houver fermento
            }
            return numerosSorte;
        }

        public static NotaFiscal FromDto(NotaFiscalDTO dto, string usuarioId = "35288343748")//remover cpf apenas teste 
        {
            return new NotaFiscal
            {
                UsuarioId = usuarioId,
                NotaCupom = dto.NotaCupom,
                Cnpj = dto.Cnpj,
                DataCompra = dto.DataCompra,
                Produtos = dto.Produtos.Select(p => Produto.FromDto(p, dto.NotaCupom)).ToList()
            };
        }       

        public NotaFiscalDTO ToDto()
        {
            return new NotaFiscalDTO
            {
                NotaCupom = this.NotaCupom,
                Cnpj = this.Cnpj,
                DataCompra = this.DataCompra,
                Produtos = this.Produtos.Select(p => p.ToDto()).ToList()
            };
        }
        public NotaFiscalDTO ToDto(List<NumeroSorteDTO> numeros)
        {

            return new NotaFiscalDTO
            {
                NotaCupom = this.NotaCupom,
                Cnpj = this.Cnpj,
                DataCompra = this.DataCompra,
                Produtos = this.Produtos.Select(p => p.ToDto()).ToList(),
                Numeros = numeros
            };
        }
    }

    public class NotaFiscalDTO
    {
        [MaxLength(11)]
        [SwaggerIgnore]
        [JsonIgnore]
        public string UsuarioId { get; set; }
        [Key]
        [Required]
        public string NotaCupom { get; set; } // Chave primária junto com UsuarioId

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }

        public List<ProdutoDTO>? Produtos { get; set; }

        public List<NumeroSorteDTO>? Numeros { get; set; }

        public IFormFile? Imagem { get; set; }
        
    }    
}
