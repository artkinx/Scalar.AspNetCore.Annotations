#if NET8_0_OR_GREATER
#if NET9_0 || NET8_0
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

        // 2. Handle Order Attribute
        new OrderProcessor().ProcessAsync(schema, context);
    }

#if NET10_0
    /// <inheritdoc/>
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        Console.WriteLine(schema.ToString());
        if (context.Type == null) return;

        // On NET10, Swashbuckle passes the concrete OpenApiSchema for component schemas
        // (e.g. WeatherForecast in components/schemas). Its Properties dictionary is
        // already populated, so we use it directly to allow OrderProcessor to mutate it.
        if (schema is OpenApiSchema concreteSchema)
        {
            // 1. Handle Class-Level / Property-Level Attributes
            new SchemaProcessor().ProcessAsync(concreteSchema, context);

            Console.WriteLine("=== PROCESSING ORDER ATTRIBUTES ===");

            // 2. Order Attribute — mutates property schemas in-place
            new OrderProcessor().ProcessAsync(concreteSchema, context);

            Console.WriteLine("=== DONE PROCESSING ORDER ATTRIBUTES ===");
            return;
        }

        // Fallback: schema is a reference — resolve its properties via the reference.
        if (schema is not OpenApiSchemaReference openSchemaRef) return;

        var openSchema = new OpenApiSchema
        {
            Type = openSchemaRef.Type,
            Format = openSchemaRef.Format,
            Description = openSchemaRef.Description,
            Example = openSchemaRef.Example,
            Properties = openSchemaRef.Properties,
            Required = openSchemaRef.Required,
            Vocabulary = openSchemaRef.Vocabulary,
            Const = openSchemaRef.Const,
            AdditionalProperties = openSchemaRef.AdditionalProperties,
            AdditionalPropertiesAllowed = openSchemaRef.AdditionalPropertiesAllowed,
            AllOf = openSchemaRef.AllOf,
            AnyOf = openSchemaRef.AnyOf,
            Comment = openSchemaRef.Comment,
            Default = openSchemaRef.Default,
            Definitions = openSchemaRef.Definitions,
            DependentRequired = openSchemaRef.DependentRequired,
            Deprecated = openSchemaRef.Deprecated,
            Discriminator = openSchemaRef.Discriminator,
            DynamicAnchor = openSchemaRef.DynamicAnchor,
            DynamicRef = openSchemaRef.DynamicRef,
            Enum = openSchemaRef.Enum,
            Examples = openSchemaRef.Examples,
            ExclusiveMaximum = openSchemaRef.ExclusiveMaximum,
            ExclusiveMinimum = openSchemaRef.ExclusiveMinimum,
            Extensions = openSchemaRef.Extensions,
            ExternalDocs = openSchemaRef.ExternalDocs,
            Id = openSchemaRef.Id,
            Items = openSchemaRef.Items,
            Maximum = openSchemaRef.Maximum,
            MaxItems = openSchemaRef.MaxItems,
            MaxLength = openSchemaRef.MaxLength,
            MaxProperties = openSchemaRef.MaxLength,
            Minimum = openSchemaRef.Minimum,
            MinItems = openSchemaRef.MinItems,
            MinLength = openSchemaRef.MinLength,
            MinProperties = openSchemaRef.MinProperties,
            //Metadata = openSchemaRef.Metadata,
            MultipleOf = openSchemaRef.MultipleOf,
            Not = openSchemaRef.Not,
            OneOf = openSchemaRef.OneOf,
            Pattern = openSchemaRef.Pattern,
            PatternProperties = openSchemaRef.PatternProperties,
            ReadOnly = openSchemaRef.ReadOnly,
            Schema = openSchemaRef.Schema,
            Title = openSchemaRef.Title,
            UnevaluatedProperties = openSchemaRef.UnevaluatedProperties,
            UnevaluatedPropertiesSchema = openSchemaRef.UnevaluatedPropertiesSchema,
            UniqueItems = openSchemaRef.UniqueItems,
            UnrecognizedKeywords = openSchemaRef.UnrecognizedKeywords,
            WriteOnly = openSchemaRef.WriteOnly,
            Xml = openSchemaRef.Xml
        };

        // 1. Handle Class-Level Attribute
        new SchemaProcessor().ProcessAsync(openSchema, context);

        // 2. Order Attribute
        new OrderProcessor().ProcessAsync(openSchema, context);
    }
#endif
}

//#elif NET8_0
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//public class ScalarSwashbuckleSchemaFilter : ISchemaFilter
//{
//    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
//    {
//        throw new NotImplementedException();
//    }
//}

#endif