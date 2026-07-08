
#if NET9_0 || NET8_0
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#elif NET10_0_OR_GREATER
using Microsoft.OpenApi;
#endif

using System.Reflection;
using System.Text.Json.Nodes;
using Swashbuckle.AspNetCore.SwaggerGen;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

namespace Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

/// <summary>
/// Implements a Swashbuckle schema filter that inspects type metadata for the <see cref="ScalarSchemaAttribute"/> and enriches the OpenAPI schema definitions with corresponding descriptions and mock values. This filter allows developers to easily integrate Scalar-specific metadata into their API documentation by simply adding the <see cref="ScalarSchemaAttribute"/> to their C# classes or properties, enabling enhanced visualization and organization in the Scalar UI. By applying this filter, any schema that has the <see cref="ScalarSchemaAttribute"/> will have its description and example values properly reflected in the generated OpenAPI documentation, improving the overall developer experience when using the Scalar UI to explore API schemas.
/// </summary>
#if NET9_0 || NET8_0
public class ScalarSwashbuckleSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Applies the filter to enrich the OpenAPI schema based on the presence of the <see cref="ScalarSchemaAttribute"/> on the target type or its properties. The filter checks for the attribute at both the class and property levels, updating the schema's description and example values accordingly. This allows for seamless integration of Scalar-specific metadata into the API documentation, enhancing the clarity and usability of the generated OpenAPI specifications for clients consuming the API through the Scalar UI.
    /// </summary>
    /// <param name="schema">
    /// The OpenAPI schema to be enriched.
    /// </param>
    /// <param name="context">
    /// The context for the schema filter.
    /// </param>

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == null) return;

        // 1. Handle Class-Level Attribute
        var classAttribute = context.Type.GetCustomAttribute<ScalarSchemaAttribute>();
        if (classAttribute != null)
        {
            if (!string.IsNullOrEmpty(classAttribute.Description))
                schema.Description = classAttribute.Description;
        }

        // 2. Handle Property-Level Attributes
        if (schema.Properties == null) return;

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
                    schemaProperty.Format = propAttribute.Format;

                if (propAttribute.ReadOnly)

                    schemaProperty.ReadOnly = true;
                if (propAttribute.WriteOnly)

                    schemaProperty.WriteOnly = true;
                if (propAttribute.MockValue != null)
                {
                    // Scalar naturally consumes the standard OpenAPI 'example' property for its UI client
                    schemaProperty.Example = new OpenApiString(propAttribute.MockValue.ToString());

                    // Fallback specific extension if needed
                    schemaProperty.Extensions["x-scalar-mock"] = new OpenApiString(propAttribute.MockValue.ToString());
                }
            }
        }
    }
}
#endif
#if NET10_0
public class ScalarSwashbuckleSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Applies the filter to enrich the OpenAPI schema based on the presence of the <see cref="ScalarSchemaAttribute"/> on the target type or its properties. The filter checks for the attribute at both the class and property levels, updating the schema's description and example values accordingly. This allows for seamless integration of Scalar-specific metadata into the API documentation, enhancing the clarity and usability of the generated OpenAPI specifications for clients consuming the API through the Scalar UI.
    /// </summary>
    /// <param name="schema">
    /// The OpenAPI schema to be enriched.
    /// </param>
    /// <param name="context">
    /// The context for the schema filter.
    /// </param>

    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == null) return;

        // 1. Handle Class-Level Attribute
        var classAttribute = context.Type.GetCustomAttribute<ScalarSchemaAttribute>();
        if (classAttribute != null)
        {
            if (!string.IsNullOrEmpty(classAttribute.Description))
                schema.Description = classAttribute.Description;
        }

        // 2. Handle Property-Level Attributes
        if (schema.Properties == null) return;

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
                                
                if(schemaProperty is OpenApiSchema concreteSchema){
                            if (!string.IsNullOrEmpty(propAttribute.Description))
                    concreteSchema.Description = propAttribute.Description;

                if (!string.IsNullOrEmpty(propAttribute.Format))
                    concreteSchema.Format = propAttribute.Format;

                if (propAttribute.ReadOnly)
                    concreteSchema.ReadOnly = true;
                                        
                if (propAttribute.WriteOnly)
                    concreteSchema.WriteOnly = true;
                                        
                if (propAttribute.MockValue != null)
                {
                    // Scalar naturally consumes the standard OpenAPI 'example' property for its UI client
                    // concreteSchema.Example = JsonValue.Create(propAttribute.MockValue.ToString());

                    // Fallback specific extension if needed
                    schemaProperty?.Extensions?["x-scalar-mock"] = new JsonNodeExtension(JsonValue.Create(propAttribute.MockValue.ToString() ?? ""));
                }
                }
            }
        }
    }
}
#endif