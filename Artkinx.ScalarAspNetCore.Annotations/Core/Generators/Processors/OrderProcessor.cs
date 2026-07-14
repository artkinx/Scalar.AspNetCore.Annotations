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
    internal class OrderProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var type = context.JsonTypeInfo.Type;

                if (type == null)
                {
                    return Task.CompletedTask;
                }

                var props = type.GetProperties();

                if (props == null || props.Length == 0) return Task.CompletedTask;

                var orderedProps = props
                    .Select(p => (Attribute: p.GetCustomAttribute<ScalarOrderAttribute>(), Property: p))
                    .Where(x => x.Attribute != null)
                    .ToList();

                if (orderedProps.Count == 0) return Task.CompletedTask;

                foreach (var (orderAttr, property) in orderedProps)
                {
                    Console.WriteLine(property.Name);

                    // 3. Look up the EXISTING property schema using a case-insensitive key match.
                    //    Swashbuckle typically camelCases property names.
                    //    NEVER call context.SchemaGenerator.GenerateSchema() here — that
                    //    re-invokes all schema filters including this one → StackOverflowException.
                    var schemaPropertyKey = schema.Properties?.Keys
                        .FirstOrDefault(k => k.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                    if (schemaPropertyKey == null)
                    {
                        continue;
                    }

                    var schemaProperty = schema.Properties?[schemaPropertyKey];

                    if (schemaProperty == null || orderAttr == null)
                    {
                        continue;
                    }

                    // 4. Add x-order extension using the correct type per target framework.
                    //    On NET10, IOpenApiSchema.Extensions is read-only on the interface —
                    //    we must cast to the concrete OpenApiSchema to mutate it.
#if NET10_0
                    if (schemaProperty is OpenApiSchema concreteSchema10)
                    {
                        concreteSchema10.Extensions ??= new Dictionary<string, IOpenApiExtension>();
                        concreteSchema10.Extensions["x-order"] = new JsonNodeExtension(JsonValue.Create(orderAttr.Order));
                    }
#elif NET9_0

                    schemaProperty.Extensions ??= new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
                    schemaProperty.Extensions["x-order"] = new OpenApiInteger(orderAttr.Order);
#endif
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the order attribute: {ex.Message}");
            }

            return Task.CompletedTask;
        }
#endif
        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks and adds the x-order extension to the properties of the schema if they carry the <see cref="ScalarOrderAttribute"/>.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Ensure the type and schema properties exist.
                //    Early-return if null — nothing to annotate.
                if (context.Type == null) return Task.CompletedTask;
                if (schema.Properties == null) return Task.CompletedTask;

                var props = context.Type.GetProperties();

                // 2. Filter down to only the properties that actually carry [ScalarOrder].
                //    Do NOT iterate all properties — those without the attribute have a null
                //    attribute reference and would throw on .Order access.
                var orderedProps = props
                    .Select(p => (Attribute: p.GetCustomAttribute<ScalarOrderAttribute>(), Property: p))
                    .Where(x => x.Attribute != null)
                    .ToList();

                if (orderedProps.Count == 0) return Task.CompletedTask;

                foreach (var (orderAttr, property) in orderedProps)
                {
                    Console.WriteLine(property.Name);

                    // 3. Look up the EXISTING property schema using a case-insensitive key match.
                    //    Swashbuckle typically camelCases property names.
                    //    NEVER call context.SchemaGenerator.GenerateSchema() here — that
                    //    re-invokes all schema filters including this one → StackOverflowException.
                    var schemaPropertyKey = schema.Properties.Keys
                        .FirstOrDefault(k => k.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                    if (schemaPropertyKey == null)
                    {
                        continue;
                    }

                    var schemaProperty = schema.Properties[schemaPropertyKey];

                    if (schemaProperty == null || orderAttr == null)
                    {
                        continue;
                    }

                    // 4. Add x-order extension using the correct type per target framework.
                    //    On NET10, IOpenApiSchema.Extensions is read-only on the interface —
                    //    we must cast to the concrete OpenApiSchema to mutate it.
#if NET10_0
                    if (schemaProperty is OpenApiSchema concreteSchema10)
                    {
                        concreteSchema10.Extensions ??= new Dictionary<string, IOpenApiExtension>();
                        concreteSchema10.Extensions["x-order"] = new JsonNodeExtension(JsonValue.Create(orderAttr.Order));
                    }
#elif NET9_0 || NET8_0
                    schemaProperty.Extensions ??= new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
                    schemaProperty.Extensions["x-order"] = new OpenApiInteger(orderAttr.Order);
#endif
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the order attribute: {ex.Message}");
            }
            finally
            {
                if (schema.Properties != null)
                {
                    Console.WriteLine($"Done processing Order Attribute, Properties: {string.Join(",", schema.Properties.Keys)}");
                }
            }
            return Task.CompletedTask;
        }
    }
}
