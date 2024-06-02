using CadastroAPI.Models;
using FluentValidation;

namespace CadastroAPI.Validators
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 100 caracteres.");

            RuleFor(p => p.Versao)
                .NotEmpty().WithMessage("A versão do produto é obrigatória.")
                .MaximumLength(10).WithMessage("A versão do produto deve ter no máximo 10 caracteres.");
        }
    }

    public class NotaFiscalValidator : AbstractValidator<NotaFiscal>
    {
        public NotaFiscalValidator()
        {
            RuleFor(n => n.UsuarioId)
              .NotEmpty().WithMessage("O Cpf do usuario é obrigatório.")
              .MaximumLength(11).WithMessage("O número do cupom deve ter no máximo 11 caracteres.");

            RuleFor(n => n.NotaCupom)
                .NotEmpty().WithMessage("O número do cupom é obrigatório.")
                .MaximumLength(20).WithMessage("O número do cupom deve ter no máximo 20 caracteres.");

            RuleFor(n => n.Cnpj)
                .NotEmpty().WithMessage("O CNPJ é obrigatório.")
                .Matches(@"^\d{14}$").WithMessage("O CNPJ deve conter 14 dígitos.")
                .Must(IsValidCnpj).WithMessage("O CNPJ informado não é válido.");

            RuleFor(n => n.DataCompra)
                .NotEmpty().WithMessage("A data da compra é obrigatória.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data da compra não pode ser no futuro.");

            //RuleFor(n => n.Imagem)
            //    .NotNull().WithMessage("A imagem é obrigatória.");

            RuleForEach(n => n.Produtos)
                .SetValidator(new ProdutoValidator());
        }

        private bool IsValidCnpj(string cnpj)
        {
            CNPJValidator cnpjValido = new(cnpj);

            return cnpjValido.IsValid();
        }
    }

}
