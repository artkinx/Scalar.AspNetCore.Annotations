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
    internal class OperationProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            var scalarOp = context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarOperationAttribute>().FirstOrDefault();



            if (scalarOp != null)
            {
                if (!string.IsNullOrEmpty(scalarOp.Summary))
                    operation.Summary = scalarOp.Summary;

                if (!string.IsNullOrEmpty(scalarOp.Description))
                    operation.Description = scalarOp.Description;

                if (!string.IsNullOrEmpty(scalarOp.OperationId))
                    operation.OperationId = scalarOp.OperationId;

                if (scalarOp.Tags?.Any() is true)
                {
#if NET10_0
                    operation.Tags = scalarOp.Tags.Select(selector: t => new OpenApiTagReference(referenceId: t)).ToHashSet();
#elif NET9_0
                operation.Tags = [.. scalarOp.Tags.Select(selector: t => new OpenApiTag() { Name = t })];
#endif
                }

                if (!string.IsNullOrEmpty(scalarOp.ThemeColor))
                {
                    // In Scalar, color isn't a native property, so we use their recognized x-extension or tag it
#if NET9_0
                operation.Extensions["x-scalar-color"] = new OpenApiString(scalarOp.ThemeColor);

#elif NET10_0
                    if (operation.Extensions == null)
                    {
                        operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                    }
                    operation.Extensions?["x-scalar-color"] = new JsonNodeExtension(JsonValue.Create(scalarOp.ThemeColor));
#endif
                }
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
            var scalarOp = context.MethodInfo.GetCustomAttribute<ScalarOperationAttribute>();

            if (scalarOp == null)
                return Task.CompletedTask;
            Console.WriteLine($"Processing ScalarOperationAttribute for method: {context.MethodInfo.Name}");

            if (!string.IsNullOrEmpty(scalarOp.Description))
                operation.Description = scalarOp.Description;

            if (!string.IsNullOrEmpty(scalarOp.Summary))
                operation.Summary = scalarOp.Summary;

            if (!string.IsNullOrEmpty(scalarOp.OperationId))
                operation.OperationId = scalarOp.OperationId;

            if (scalarOp.Tags != null)
            {
#if NET10_0
                operation.Tags = scalarOp.Tags.Select(selector: t => new OpenApiTagReference(referenceId: t)).ToHashSet();
#elif NET9_0
                    operation.Tags = [.. scalarOp.Tags.Select(selector: t => new OpenApiTag() { Name = t })];
#endif
            }

            if (!string.IsNullOrEmpty(scalarOp.ThemeColor))
            {
                // In Scalar, color isn't a native property, so we use their recognized x-extension or tag it
#if NET9_0
                    operation.Extensions["x-scalar-color"] = new OpenApiString(scalarOp.ThemeColor);

#elif NET10_0
                if (operation.Extensions == null)
                {
                    operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                operation.Extensions?["x-scalar-color"] = new JsonNodeExtension(JsonValue.Create(scalarOp.ThemeColor));
#endif
            }

            Console.WriteLine($"Description: {operation.Description}, Summary: {operation.Summary}, OperationId: {operation.OperationId}");

            return Task.CompletedTask;
        }

        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
