using CadastroAPI.Models;
using FluentValidation;

namespace CadastroAPI.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Nome).NotEmpty().WithMessage("Nome é obrigatória.");
            RuleFor(u => u.CPF).NotEmpty().Must(IsAValidCPF).WithMessage("CPF inválido");
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.DataNascimento).NotEmpty().Must(BevalidaIdade).WithMessage("É necessário ter ao menos 18 anos para participar");
            RuleFor(u => u.Estado).NotEmpty().WithMessage("Estado é obrigatória")
                                  .Length(2).WithMessage("A UF deve ter exatamente 2 caracteres.")
                                  .Must(SiglaUFValida).WithMessage("A UF informada não é válida.");
            //RuleFor(u => u.CEP).NotEmpty();
            //RuleFor(u => u.Cidade).NotEmpty();
            //RuleFor(u => u.Endereco).NotEmpty();
            //RuleFor(u => u.Numero).NotEmpty();
            RuleFor(u => u.TelefoneCelular).NotEmpty().Matches(@"^\d{10,11}$").WithMessage("o numero informado não é válido.");
            RuleFor(u => u.Senha).NotEmpty();
        }

        private bool IsAValidCPF(string cpf)
        {
            CpfValidator cpfValido = new(cpf);
            if (cpfValido.IsValid())
            {
                return true;
            }
            return false;
        }
        private bool BevalidaIdade(DateTime data)
        {
            DateTime dataAtual = DateTime.Now;

            // Calculando a diferença de anos
            int idade = dataAtual.Year - data.Year;

            // Verificando a diferença de meses e dias
            if (dataAtual < data.AddYears(idade)) { idade--; }

            if (idade >= 18) { return true; }

            return false;
        }
        private bool SiglaUFValida(string uf)
        {
            string[] siglasValidas = { "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SP", "SE", "TO" };
            return Array.Exists(siglasValidas, sigla => sigla.Equals(uf, StringComparison.OrdinalIgnoreCase));
        }
    }

}
