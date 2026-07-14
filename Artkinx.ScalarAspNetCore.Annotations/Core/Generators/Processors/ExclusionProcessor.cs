using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using System.Text.Json.Nodes;
using System.Reflection;



#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors
{
    internal class ExclusionProcessor : IAttributeProcessor
    {

#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarExcludeAttribute>().Any())
            {
#if NET9_0
            operation.Extensions["x-scalar-ignore"] = new OpenApiBoolean(true);
#elif NET10_0
                if (operation.Extensions == null)
                {
                    operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                operation.Extensions["x-scalar-ignore"] = new JsonNodeExtension(JsonValue.Create(true));
#endif
            }
            return Task.CompletedTask;
        }

        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            var metedata = context.JsonTypeInfo.Type.GetCustomAttribute<ScalarExcludeAttribute>();
            if (metedata != null)
            {
#if NET9_0
            schema.Extensions["x-scalar-ignore"] = new OpenApiBoolean(true);
#elif NET10_0
                if (schema.Extensions == null)
                {
                    schema.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                schema.Extensions["x-scalar-ignore"] = new JsonNodeExtension(JsonValue.Create(true));
#endif
            }

            return Task.CompletedTask;
        }
#endif

        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            var excludeAttribute = context.MethodInfo.GetCustomAttribute<ScalarExcludeAttribute>();
            if (excludeAttribute != null)
            {
#if NET9_0
            operation.Extensions["x-scalar-ignore"] = new OpenApiBoolean(true);
#elif NET10_0
                if (operation.Extensions == null)
                {
                    operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                operation.Extensions?["x-scalar-ignore"] = new JsonNodeExtension(JsonValue.Create(true));
#endif
            }

            return Task.CompletedTask;
        }
        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            var excludeAttribute = context.Type.GetCustomAttribute<ScalarExcludeAttribute>();
            if (excludeAttribute != null)
            {
#if NET9_0
                schema.Extensions["x-scalar-ignore"] = new OpenApiBoolean(true);
#elif NET10_0
                if (schema.Extensions == null)
                {
                    schema.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                schema.Extensions?["x-scalar-ignore"] = new JsonNodeExtension(JsonValue.Create(true));
#endif
            }
            return Task.CompletedTask;

        }
    }
}
