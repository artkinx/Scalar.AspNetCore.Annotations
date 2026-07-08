using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

using System.Reflection;
using System.Text.Json.Nodes;


#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors
{
    internal class SchemaProcessor : IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
        {
            var type = context.JsonTypeInfo.Type;

#if NET9_0
            if (schema.Extensions == null)
            {
                schema.Extensions = new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
            }
#elif NET10_0
            if (schema.Extensions == null)
            {
                schema.Extensions = new Dictionary<string, IOpenApiExtension>();
            }
#endif

            var schemaAttribute = type.GetCustomAttribute<ScalarSchemaAttribute>();

            if (schemaAttribute != null)
            {
                if (!string.IsNullOrEmpty(schemaAttribute.Description))
                {
                    schema.Description = schemaAttribute.Description;
                }

                // We can also inject scalar-specific extensions here
                if (schemaAttribute.MockValue != null)
                {
                    // Handle injecting the mock value into the schema extension or example property
#if NET9_0
                    schema.Extensions["x-scalar-mock"] = new OpenApiString(schemaAttribute.MockValue.ToString());
#elif NET10_0
                    schema.Extensions?["x-scalar-mock"] = new JsonNodeExtension(JsonValue.Create(schemaAttribute.MockValue.ToString() ?? ""));
#endif
                }

                // TODO: Complete the refrence to the enum varnames
                if (type.IsEnum)
                {
                    var t = type.GetEnumUnderlyingType();
                    var enumAttribute = type.GetCustomAttribute<ScalarEnumAttribute>();
                    if (enumAttribute != null)
                    {
                        if (!string.IsNullOrEmpty(enumAttribute.Description))
                        {
                            schema.Description = enumAttribute.Description;
                        }
                        schema.Title = enumAttribute.Title;
                    }
                }
            }

            return Task.CompletedTask;
        }
#endif

        public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
        {
            var classAttribute = context.Type.GetCustomAttribute<ScalarSchemaAttribute>();
            if (classAttribute != null)
            {
                if (!string.IsNullOrEmpty(classAttribute.Description))
                    schema.Description = classAttribute.Description;
            }

            // 2. Handle Property-Level Attributes
            if (schema.Properties == null) return Task.CompletedTask;

            var properties = context.Type.GetProperties();
            foreach (var property in properties)
            {
                var propAttribute = property.GetCustomAttribute<ScalarSchemaAttribute>();
                if (propAttribute == null) continue;

                // Swashbuckle typically camelCases schema properties. 
                // We use case-insensitive matching to find the exact OpenAPI property key.
                var schemaPropertyKey = schema.Properties.Keys
                    .FirstOrDefault(k => k.Equals(property.Name, StringComparison.OrdinalIgnoreCase));

                if (schemaPropertyKey != null)
                {
                    var schemaProperty = schema.Properties[schemaPropertyKey];

                    if (!string.IsNullOrEmpty(propAttribute.Description))
                        schemaProperty.Description = propAttribute.Description;

                    if (!string.IsNullOrEmpty(propAttribute.Format))
#if NET9_0 || NET8_0
                        schemaProperty.Format = propAttribute.Format;
#elif NET10_0
                        schema.Format = propAttribute.Format;
#endif

                    if (propAttribute.ReadOnly)
                    {
#if NET9_0 || NET8_0

                        schemaProperty.ReadOnly = true;
#elif NET10_0
                        schema.ReadOnly = propAttribute.ReadOnly;
#endif
                    }

                    if (propAttribute.WriteOnly)
                    {
#if NET9_0 || NET8_0

                        schemaProperty.WriteOnly = true;

#elif NET10_0        
                        schema.WriteOnly = true;
#endif
                    }
                    if (propAttribute.MockValue != null)
                    {
#if NET9_0 || NET8_0

                        // Scalar naturally consumes the standard OpenAPI 'example' property for its UI client
                        schemaProperty.Example = new OpenApiString(propAttribute.MockValue.ToString());

                        // Fallback specific extension if needed
                        schemaProperty.Extensions["x-scalar-mock"] = new OpenApiString(propAttribute.MockValue.ToString());

#elif NET10_0                        
                        schemaProperty?.Extensions?["x-scalar-mock"] = new JsonNodeExtension(JsonValue.Create(propAttribute.MockValue.ToString() ?? ""));
#endif
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
