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
    internal class StabilityProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            var metadata = context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarStabilityAttribute>();
            if (metadata.Any())
            {
                var stabilityAttr = metadata.OfType<ScalarStabilityAttribute>().First();
#if NET9_0
            operation.Extensions["x-scalar-stability"] = new OpenApiString(stabilityAttr.Level.ToString().ToLowerInvariant());
#elif NET10_0
                operation.Extensions!["x-scalar-stability"] = new JsonNodeExtension(JsonValue.Create(stabilityAttr.Level.ToString().ToLowerInvariant()));
#endif
            }

            return Task.CompletedTask;
        }

        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
#endif

        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            var metadata = context.MethodInfo.GetCustomAttributes<ScalarStabilityAttribute>();
            if (metadata.Any())
            {
                var stabilityAttr = metadata.OfType<ScalarStabilityAttribute>().First();
#if NET9_0
            operation.Extensions["x-scalar-stability"] = new OpenApiString(stabilityAttr.Level.ToString().ToLowerInvariant());
#elif NET10_0
                operation.Extensions!["x-scalar-stability"] = new JsonNodeExtension(JsonValue.Create(stabilityAttr.Level.ToString().ToLowerInvariant()));
#endif
            }

            return Task.CompletedTask;
        }

        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
