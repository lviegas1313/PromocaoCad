using CadastroAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CadastroAPI.ModelBinders
{
    public class ProdutoDTOModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(ProdutoDTO))
            {
                return new ProdutoDTOModelBinder();
            }

            return null;
        }
    }
}
