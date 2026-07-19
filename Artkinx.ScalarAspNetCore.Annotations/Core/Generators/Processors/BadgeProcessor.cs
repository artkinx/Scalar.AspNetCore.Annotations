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
    internal class BadgeProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            var badges = context.Description.ActionDescriptor.EndpointMetadata.OfType<ScalarBadgeAttribute>().ToList();
            if (badges.Count > 0)
            {
#if NET9_0
                var badgeArray = new OpenApiArray();
                foreach (var badge in badges)
                {
                    // Injecting Scalar's expected x- extension format
                    badgeArray.Add(new OpenApiObject
                    {
                        ["name"] = new OpenApiString(badge.Name),
                        ["position"] = new OpenApiString(badge.Position.ToString().ToLowerInvariant()),
                        ["color"] = new OpenApiString(badge.Color)
                    });
                }
                operation.Extensions["x-badges"] = badgeArray;
#elif NET10_0
                var badgeArray = new JsonArray();
                foreach (var badge in badges)
                {
                    badgeArray.Add(new JsonObject()
                    {
                        ["name"] = JsonValue.Create(badge.Name),
                        ["position"] = JsonValue.Create(badge.Position.ToString().ToLowerInvariant()),
                        ["color"] = JsonValue.Create(badge.Color)
                    });
                }
                operation.Extensions ??= new Dictionary<string, IOpenApiExtension>();
                operation.Extensions["x-badges"] = new JsonNodeExtension(badgeArray);
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
            var badges = context.MethodInfo.GetCustomAttributes<ScalarBadgeAttribute>().ToList();
            if (badges.Count > 0)
            {
                Console.WriteLine($"Processing {badges.Count} badge(s) for operation {operation.OperationId}");
#if NET9_0
                var badgeArray = new OpenApiArray();
                foreach (var badge in badges)
                {
                    // Injecting Scalar's expected x- extension format
                    badgeArray.Add(new OpenApiObject
                    {
                        ["name"] = new OpenApiString(badge.Name),
                        ["position"] = new OpenApiString(badge.Position.ToString().ToLowerInvariant()),
                        ["color"] = new OpenApiString(badge.Color)
                    });
                }
                operation.Extensions["x-badges"] = badgeArray;
#elif NET10_0
                var badgeArray = new JsonArray();
                foreach (var badge in badges)
                {
                    badgeArray.Add(new JsonObject()
                    {
                        ["name"] = JsonValue.Create(badge.Name),
                        ["position"] = JsonValue.Create(badge.Position.ToString().ToLowerInvariant()),
                        ["color"] = JsonValue.Create(badge.Color)
                    });
                }
                if (operation.Extensions == null)
                {
                    operation.Extensions = new Dictionary<string, IOpenApiExtension>();
                }
                operation.Extensions["x-badges"] = new JsonNodeExtension(badgeArray);
                Console.WriteLine($"Added {badges.Count} badge(s) to operation {operation.OperationId} with Extensions: {string.Join(", ", operation.Extensions.Keys)} ANd value names: {string.Join(", ", string.Join(", ", operation.Extensions.Values.Select(v => v.ToString())))}");
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
