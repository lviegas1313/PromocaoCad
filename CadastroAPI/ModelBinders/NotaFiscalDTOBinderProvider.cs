using CadastroAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
namespace CadastroAPI.ModelBinders
{


    namespace CadastroAPI.ModelBinders
    {
        public class NotaFiscalDTOBinderProvider : IModelBinderProvider
        {
            public IModelBinder? GetBinder(ModelBinderProviderContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                if (context.Metadata.ModelType == typeof(NotaFiscalDTO))
                {
                    return new NotaFiscalDTOBinder();// NotaFiscalDTOModelBinder();
                }

                return null;
            }
        }
    }

}
