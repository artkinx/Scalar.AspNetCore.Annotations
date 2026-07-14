using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators;
using System.Reflection;







#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors
{
    internal class ResponseProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            var scalarResponse = context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarResponseAttribute>().ToList();

            scalarResponse?.ForEach(async r =>
            {
                var statusCode = r.StatusCode.ToString();
                var t = r.Type;

                if (operation.Responses.ContainsKey(statusCode))
                {
                    var response = operation.Responses[statusCode];
                    if (!string.IsNullOrEmpty(r.Description))
                    {
                        response.Description = r.Description;
                    }

                    if (!string.IsNullOrEmpty(r.ContentTypes?.FirstOrDefault()))
                    {
                        foreach (var ct in r.ContentTypes)
                        {
#if NET9_0
                            var schema = ApiSchemaGenerator.GenerateSchemaFromType(t);
#elif NET10_0
                            var schema = await context.GetOrCreateSchemaAsync(t);
#endif
                            if (!response.Content.ContainsKey(ct))
                            {
                                response.Content[ct] = new OpenApiMediaType
                                {
                                    Schema = schema,
                                    Encoding = new Dictionary<string, OpenApiEncoding>()
                                    {
                                        [ct] = new OpenApiEncoding()
                                        {
                                            ContentType = ct,
                                            Style = ct?.ToLowerInvariant()?.Contains("form") ?? false ? ParameterStyle.Form : ParameterStyle.Simple
                                        }
                                    },

                                };
                            }
                        }
                        Console.WriteLine($"{statusCode} - Content Types: {string.Join(", ", response.Content?.Keys ?? [])}, Name: {t.Name}");
                        //response.Content = r.ContentTypes.ToDictionary(ct => ct, ct => new OpenApiMediaType());
                    }
                }
            });
            return Task.CompletedTask;

        }

        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
#endif
        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            var scalarResponse = context.MethodInfo.GetCustomAttributes<ScalarResponseAttribute>().ToList();

            if (scalarResponse != null && scalarResponse.Any())
            {
                foreach (var r in scalarResponse)
                {
                    var statusCode = r.StatusCode.ToString();
                    var t = r.Type;
                    if (operation.Responses.ContainsKey(statusCode))
                    {
                        var response = operation.Responses[statusCode];
                        if (!string.IsNullOrEmpty(r.Description))
                        {
                            response.Description = r.Description;
                        }
                        if (!string.IsNullOrEmpty(r.ContentTypes?.FirstOrDefault()))
                        {
                            foreach (var ct in r.ContentTypes)
                            {
#if NET9_0 || NET8_0
                            var schema = ApiSchemaGenerator.GenerateSchemaFromType(t);
#elif NET10_0
                            var schema = context.SchemaGenerator.GenerateSchema(t, context.SchemaRepository);
#endif


                                if (!response.Content.ContainsKey(ct))
                                {
                                    response.Content[ct] = new OpenApiMediaType
                                    {
                                        Schema = schema,
                                        Encoding = new Dictionary<string, OpenApiEncoding>()
                                        {
                                            [ct] = new OpenApiEncoding()
                                            {
                                                ContentType = ct,
                                                Style = ct?.ToLowerInvariant()?.Contains("form") ?? false ? ParameterStyle.Form : ParameterStyle.Simple
                                            }
                                        },

                                    };
                                }
                            }
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
