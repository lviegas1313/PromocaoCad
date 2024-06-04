using CadastroAPI.Models;
using FluentValidation;

namespace CadastroAPI.Validators
{    
    public class ProdutoValidator : AbstractValidator<ProdutoDTO>
    {
        public ProdutoValidator()
        {
            RuleFor(dto => dto.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 100 caracteres.");

            //RuleFor(dto => dto.Versao)
            //    .MaximumLength(50).WithMessage("A versão do produto deve ter no máximo 50 caracteres.");

            RuleFor(dto => dto.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade do produto deve ser maior que zero.");

            RuleFor(dto => dto.Valor)
                .GreaterThan(0).WithMessage("O valor do produto deve ser maior que zero.");
        }
    }

}
