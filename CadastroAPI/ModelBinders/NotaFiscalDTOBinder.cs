using CadastroAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

namespace CadastroAPI.ModelBinders
{
    public class NotaFiscalDTOBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.HttpContext.Request.HasFormContentType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            var form = await bindingContext.HttpContext.Request.ReadFormAsync();
            var dto = new NotaFiscalDTO();
            dto.UsuarioId = "0";
            dto.NotaCupom = form["NotaCupom"];
            dto.Cnpj = form["Cnpj"];
            dto.DataCompra = DateTime.Parse(form["DataCompra"]);
            dto.Produtos = new List<ProdutoDTO>();
            // Obter a string JSON dos produtos do formulário
            var produtosString = form["Produtos"];

            if (!string.IsNullOrEmpty(produtosString))
            {
                foreach (var item in form["Produtos"].ToList())
                {
                    var produtoDto = new ProdutoDTO();
                    produtoDto = JsonSerializer.Deserialize<ProdutoDTO>(item);
                    dto.Produtos.Add(produtoDto);
                }               
                
            }
            else
            {
                // Adicionar um produto DTO manualmente caso a lista esteja vazia
                var produtoDto = new ProdutoDTO
                {
                    Nome = form["nome"],
                    Versao = form["versao"],
                    Quantidade = Convert.ToInt32(form["quantidade"]),
                    Valor = Convert.ToDecimal(form["valor"])
                };
                dto.Produtos.Add(produtoDto);
            }



            dto.Imagem = form.Files.GetFile("Imagem");

            bindingContext.Result = ModelBindingResult.Success(dto);
        }
    }

    public class NotaFiscalDTOModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Obter o valor do campo NotaCupom
            var notaCupomValue = bindingContext.ValueProvider.GetValue("NotaCupom").FirstValue;

            // Obter o valor do campo UsuarioId
            var usuarioIdValue = bindingContext.ValueProvider.GetValue("UsuarioId").FirstValue;


            // Criar uma instância de NotaFiscalDTO e atribuir os valores
            var notaFiscalDto = new NotaFiscalDTO
            {
                NotaCupom = notaCupomValue,
                UsuarioId = usuarioIdValue,
                // Preencher os outros campos do DTO
            };

            // Definir o resultado do model binding
            bindingContext.Result = ModelBindingResult.Success(notaFiscalDto);

            return Task.CompletedTask;
        }
    }

}
