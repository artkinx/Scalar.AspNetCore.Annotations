using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

/// <summary>
/// Provides extension methods for integrating Scalar-specific annotations into the Swashbuckle OpenAPI generator pipeline. This class contains methods that can be called on the SwaggerGenOptions to add custom operation and schema filters that recognize Scalar attributes and enhance the generated API documentation accordingly, allowing for features like badges, code samples, and enhanced schema descriptions in the Scalar UI.
/// </summary>
public static class ScalarSwashbuckleExtensions
{
    /// <summary>
    /// Adds Scalar attribute filters to the Swashbuckle OpenAPI generator pipeline.
    /// </summary>
#if NET9_0 || NET8_0
    public static SwaggerGenOptions AddScalarAnnotations(this SwaggerGenOptions options)
    {

        options.OperationFilter<ScalarSwashbuckleOperationFilter>();
        options.SchemaFilter<ScalarSwashbuckleSchemaFilter>();

        return options;
    }
#elif NET10_0
    public static SwaggerGenOptions AddScalarAnnotations(this SwaggerGenOptions options)
    {

        options.OperationFilter<ScalarSwashbuckleOperationFilter>();
        options.SchemaFilter<ScalarSwashbuckleSchemaFilter>();

        return options;
    }
#endif
}