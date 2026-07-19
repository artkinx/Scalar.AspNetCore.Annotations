
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using System.Text.Json.Nodes;

using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using System.Reflection;


#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors
{
    internal class CodeSampleProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        //<inheritdoc/>
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            var codeSamples = context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarCodeSampleAttribute>().ToList();
            if (codeSamples.Count > 0)
            {
#if NET9_0
            var sampleArray = new OpenApiArray();
            foreach (var sample in codeSamples)
            {
                sampleArray.Add(new OpenApiObject
                {
                    ["lang"] = new OpenApiString(sample.Language),
                    ["source"] = new OpenApiString(sample.Code),
                    ["label"] = new OpenApiString(string.IsNullOrEmpty(sample.Title) ? sample.Language : sample.Title)
                });
            }
            operation.Extensions["x-codeSamples"] = sampleArray;
#elif NET10_0
                var sampleArray = new JsonArray();
                foreach (var sample in codeSamples)
                {
                    sampleArray.Add(new JsonObject()
                    {
                        ["lang"] = JsonValue.Create(sample.Language),
                        ["source"] = JsonValue.Create(sample.Code),
                        ["label"] = JsonValue.Create(string.IsNullOrEmpty(sample.Title) ? sample.Language : sample.Title)
                    });
                }
                operation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
                operation.Extensions["x-codeSamples"] = new JsonNodeExtension(sampleArray);
#endif
            }
            return Task.CompletedTask;
         }

        //<inheritdoc/>
        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
#endif
        //<inheritdoc/>
        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            var codeSamples = context.MethodInfo.GetCustomAttributes<ScalarCodeSampleAttribute>().ToList();
            if (codeSamples.Count > 0)
            {
#if NET9_0
            var sampleArray = new OpenApiArray();
            foreach (var sample in codeSamples)
            {
                sampleArray.Add(new OpenApiObject
                {
                    ["lang"] = new OpenApiString(sample.Language),
                    ["source"] = new OpenApiString(sample.Code),
                    ["label"] = new OpenApiString(string.IsNullOrEmpty(sample.Title) ? sample.Language : sample.Title)
                });
            }
            operation.Extensions["x-codeSamples"] = sampleArray;
#elif NET10_0
                var sampleArray = new JsonArray();
                foreach (var sample in codeSamples)
                {
                    sampleArray.Add(new JsonObject()
                    {
                        ["lang"] = JsonValue.Create(sample.Language),
                        ["source"] = JsonValue.Create(sample.Code),
                        ["label"] = JsonValue.Create(string.IsNullOrEmpty(sample.Title) ? sample.Language : sample.Title)
                    });
                }
                if (operation.Extensions == null)
                {
                    operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                operation.Extensions["x-codeSamples"] = new JsonNodeExtension(sampleArray);
#endif
            }
            return Task.CompletedTask;
        }

        //<inheritdoc/>
        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
