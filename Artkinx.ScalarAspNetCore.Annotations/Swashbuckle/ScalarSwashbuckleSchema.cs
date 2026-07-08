#if NET9_0_OR_GREATER
#if NET9_0
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
#endif

using System.Reflection;
using System.Text.Json.Nodes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;

namespace Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

/// <summary>
/// Implements a Swashbuckle schema filter that inspects type metadata for the <see cref="ScalarSchemaAttribute"/> and enriches the OpenAPI schema definitions with corresponding descriptions and mock values. This filter allows developers to easily integrate Scalar-specific metadata into their API documentation by simply adding the <see cref="ScalarSchemaAttribute"/> to their C# classes or properties, enabling enhanced visualization and organization in the Scalar UI. By applying this filter, any schema that has the <see cref="ScalarSchemaAttribute"/> will have its description and example values properly reflected in the generated OpenAPI documentation, improving the overall developer experience when using the Scalar UI to explore API schemas.
/// </summary>
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
        new SchemaProcessor().ProcessAsync(schema, context);
    }

#if NET10_0
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == null) return;

        // 1. Handle Class-Level Attribute
        new SchemaProcessor().ProcessAsync((OpenApiSchema)schema, context);

    }
#endif
}
#endif
