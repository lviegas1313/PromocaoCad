using CadastroAPI.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;


namespace CadastroAPI.Filters
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.ApiDescription.ParameterDescriptions
                .Where(p => p.ModelMetadata.ModelType == typeof(IFormFile) || p.ModelMetadata.ModelType == typeof(IFormFileCollection))
                .ToList();

            if (fileParams.Any())
            {
                foreach (var param in fileParams)
                {
                    var parameterToRemove = operation.Parameters.FirstOrDefault(p => p.Name == param.Name);
                    if (parameterToRemove != null)
                    {
                        operation.Parameters.Remove(parameterToRemove);
                    }

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
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var parameters = context.MethodInfo.GetParameters();

            if (parameters.Any(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFile[])))
            {
                var formFileParams = parameters
                    .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFile[]))
                    .ToList();

                var otherParams = parameters
                    .Where(p => p.ParameterType != typeof(IFormFile) && p.ParameterType != typeof(IFormFile[]))
                    .ToList();

                var properties = new Dictionary<string, OpenApiSchema>();

                foreach (var param in otherParams)
                {
                    var schema = context.SchemaGenerator.GenerateSchema(param.ParameterType, context.SchemaRepository);
                    properties[param.Name] = schema;
                }

                foreach (var param in formFileParams)
                {
                    properties[param.Name] = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    };
                }

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = properties,
                                Required = formFileParams.Select(p => p.Name).ToHashSet()
                            }
                        }
                    }
                };
            }
        }
    }


    public class CorrectArraySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(List<ProdutoDTO>))
            {
                schema.Type = "array";
                schema.Items = context.SchemaGenerator.GenerateSchema(typeof(ProdutoDTO), context.SchemaRepository);
            }
        }
    }

}


