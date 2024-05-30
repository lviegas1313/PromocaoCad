using System.ComponentModel.DataAnnotations;

namespace CadastroAPI.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(11)]
        public string CPF { get; set; }

        [Required]
        [MaxLength(120)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [MaxLength(2)]
        public string Estado { get; set; }

        [Required]
        [MaxLength(10)]
        public string? CEP { get; set; }

        //[Required]
        [MaxLength(120)]
        public string? Cidade { get; set; }

        //[Required]
        [MaxLength(120)]
        public string? Endereco { get; set; }

        //[Required]
        [MaxLength(15)]
        public string? Numero { get; set; }
        [MaxLength(20)]
        public string? Complemento { get; set; }

        [Required]
        [MaxLength(11)]
        public string TelefoneCelular { get; set; }

        [Required]
        [MaxLength(250)]
        public string Senha { get; set; }
    }
}
