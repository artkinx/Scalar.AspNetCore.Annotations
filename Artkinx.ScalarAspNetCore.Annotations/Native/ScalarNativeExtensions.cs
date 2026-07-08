#if NET9_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Microsoft.AspNetCore.OpenApi;

namespace Artkinx.ScalarAspNetCore.Annotations.Native;

/// <summary>
/// Provides extension methods for integrating Scalar-specific annotations into the OpenAPI generation process, allowing developers to easily enrich their API documentation with Scalar UI metadata such as badges, code samples, and custom operation details by simply adding the appropriate attributes to their API endpoints and invoking this extension method during OpenAPI configuration.
/// </summary>
public static class ScalarNativeExtensions
{
    /// <summary>
    /// Adds support for Scalar-specific annotations to the OpenAPI generation process by registering custom operation and schema transformers that recognize and process attributes like <see cref="Core.Attributes.ScalarOperationAttribute"/>, <see cref="Core.Attributes.ScalarBadgeAttribute"/>, and <see cref="Attributes.ScalarResponseAttribute"/>. This extension method should be called during the OpenAPI configuration phase (e.g., in the Startup.cs or Program.cs file) to ensure that the Scalar-specific metadata is properly incorporated into the generated OpenAPI documentation, enabling enhanced visualization and organization in the Scalar UI.
    /// </summary>
    /// <param name="options">The <see cref="OpenApiOptions"/> instance to modify.</param>
    /// <returns>The modified <see cref="OpenApiOptions"/> instance.</returns>
    public static OpenApiOptions AddScalarAnnotations(this Microsoft.AspNetCore.OpenApi.OpenApiOptions options)
    {
        options.AddOperationTransformer<ScalarOperationTransformer>();
        options.AddSchemaTransformer<ScalarSchemaTransformer>();
        
        return options;
    }
}
#endif