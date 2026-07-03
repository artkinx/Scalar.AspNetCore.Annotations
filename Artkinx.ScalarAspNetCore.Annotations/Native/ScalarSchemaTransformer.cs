#if NET9_0_OR_GREATER


using Microsoft.AspNetCore.OpenApi;
#if NET9_0
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#endif
using Artkinx.ScalarAspNetCore.Annotations.Attributes;
using System.Reflection;
using System.Text.Json.Nodes;

namespace Artkinx.ScalarAspNetCore.Annotations.Native
{
    /// <summary>
    /// Transforms OpenAPI schemas by inspecting type metadata for Scalar-specific attributes and injecting corresponding OpenAPI extensions that Swashbuckle can utilize to enhance the generated documentation with descriptions and mock values for scalar types.
    /// </summary>
    public class ScalarSchemaTransformer : IOpenApiSchemaTransformer
    {
        /// <summary>
        /// Transforms the given OpenAPI schema by checking for the presence of the <see cref="ScalarSchemaAttribute"/> on the underlying C# type and, if found, enriches the schema with the description and mock value specified in the attribute. This allows Swashbuckle to generate more informative documentation for scalar types, including custom descriptions and example values that can be displayed in the Scalar UI.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Returns a completed task after potentially modifying the provided OpenAPI schema based on the presence of the <see cref="ScalarSchemaAttribute"/> on the underlying C# type. If the attribute is found, the schema's description and extensions may be updated accordingly, allowing for enhanced documentation in the Scalar UI. If the attribute is not present, the schema remains unchanged. The method completes synchronously since it does not perform any asynchronous operations, but it returns a Task to comply with the IOpenApiSchemaTransformer interface contract.
        /// </returns>
        public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            // context.JsonTypeInfo.Type gives us the underlying C# class/record
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
    }
}
#endif