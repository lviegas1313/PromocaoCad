using CadastroAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace CadastroAPI.Models
{
    public class Usuario
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

        [MaxLength(120)]
        public string? Cidade { get; set; }

        [MaxLength(120)]
        public string? Endereco { get; set; }

        [MaxLength(15)]
        public string? Numero { get; set; }

        [MaxLength(20)]
        public string? Complemento { get; set; }

        [Required]
        [MaxLength(11)]
        public string TelefoneCelular { get; set; }

        [Required]
        public string Senha { get; private set; }

        public void AlterarSenha(string novaSenha, IPasswordHashService passwordHashService)
        {
            Senha = passwordHashService.HashPassword(novaSenha);
        }
        public void CriptografarSenha(IPasswordHashService passwordHashService)
        {
            Senha = passwordHashService.HashPassword(Senha);
        }
        public bool ValidarSenha(string senhaDigitada, IPasswordHashService passwordHashService)
        {
            return passwordHashService.VerifyPassword(senhaDigitada, Senha);
        }
        public  Usuario() { }
        public static Usuario FromDto(UserDTO userDTO)
        {
            return new Usuario
            {
                CPF = userDTO.CPF,
                Nome = userDTO.Nome,
                Email = userDTO.Email,
                DataNascimento = userDTO.DataNascimento,
                Estado = userDTO.Estado,
                CEP = userDTO.CEP,
                Cidade = userDTO.Cidade,
                Endereco = userDTO.Endereco,
                Numero = userDTO.Numero,
                Complemento = userDTO.Complemento,
                TelefoneCelular = userDTO.TelefoneCelular,
                Senha = userDTO.Senha
            };
        }
    }
    public class UserDTO
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

        [MaxLength(10)]
        public string CEP { get; set; }

        [MaxLength(120)]
        public string Cidade { get; set; }

        [MaxLength(120)]
        public string Endereco { get; set; }

        [MaxLength(15)]
        public string Numero { get; set; }

        [MaxLength(20)]
        public string Complemento { get; set; }

        [Required]
        [MaxLength(11)]
        public string TelefoneCelular { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
