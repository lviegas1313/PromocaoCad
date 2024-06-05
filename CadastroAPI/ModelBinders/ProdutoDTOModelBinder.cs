using CadastroAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace CadastroAPI.ModelBinders
{    

    public class ProdutoDTOModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Obter o valor das propriedades do ProdutoDTO do ValueProvider
            var nomeValue = bindingContext.ValueProvider.GetValue("nome").FirstValue;
            var versaoValue = bindingContext.ValueProvider.GetValue("versao").FirstValue;
            var quantidadeValue = bindingContext.ValueProvider.GetValue("quantidade").FirstValue;
            var valorValue = bindingContext.ValueProvider.GetValue("valor").FirstValue;

            // Criar uma instância de ProdutoDTO e atribuir os valores
            var produtoDto = new ProdutoDTO
            {
                Nome = nomeValue,
                Versao = versaoValue,
                Quantidade = Convert.ToInt32(quantidadeValue),
                Valor = Convert.ToDecimal(valorValue)
            };

            // Definir o resultado do model binding
            bindingContext.Result = ModelBindingResult.Success(produtoDto);

            return Task.CompletedTask;
        }
    }

}
