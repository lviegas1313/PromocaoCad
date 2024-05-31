using CadastroAPI.Services;
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


        public string? Senha { get; set; }

        private string _senhaDescriptografada;
        //public string SenhaDescriptografada
        //{
        //    get => _senhaDescriptografada;
        //    set
        //    {
        //        _senhaDescriptografada = value;
        //        SenhaCriptografada = CriptografarSenha(value);
        //    }
        //}
        //     public string SenhaCriptografada { get; private set; }
        private readonly IPasswordHashService _passwordHashService;

        public User() { }
        public User(IPasswordHashService passwordHashService)
        {
            _passwordHashService = passwordHashService;
        }
        public void CriptografarSenha(IPasswordHashService passwordHashService)
        {
            Senha = passwordHashService.HashPassword(_senhaDescriptografada);
        }

        public bool ValidarSenha(string senhaDigitada)
        {
            return _passwordHashService.VerifyPassword(senhaDigitada, _senhaDescriptografada);
        }

    }
}
