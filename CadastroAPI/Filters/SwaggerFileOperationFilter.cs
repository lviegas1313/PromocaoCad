using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CadastroAPI.Filters
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ModelMetadata.ModelType == typeof(IFormFile) || p.ModelMetadata.ModelType == typeof(IFormFileCollection))
                .ToList();

            if (!fileParams.Any())
            {
                return;
            }

            foreach (var param in fileParams)
            {
                operation.Parameters.Remove(operation.Parameters.First(p => p.Name == param.Name));
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties =
                            {
                                [param.Name] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                }
                            }
                            }
                        }
                    }
                };
            }
        }
    }

}
