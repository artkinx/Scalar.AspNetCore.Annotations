#if NET8_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
#if NET9_0 
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#elif NET8_0
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#endif
using Swashbuckle.AspNetCore.SwaggerGen;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;


namespace Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

/// <summary>
/// Implements a Swashbuckle operation filter that inspects API endpoint metadata for Scalar-specific attributes such as <see cref="ScalarOperationAttribute"/>, <see cref="ScalarExcludeAttribute"/>, and <see cref="ScalarCodeSampleAttribute"/>. This filter enriches the OpenAPI operation definitions with corresponding extensions that Swashbuckle can utilize to enhance the generated API documentation with badges, code samples, and exclusion flags, allowing for improved visualization and organization in the Scalar UI. By applying this filter, developers can easily integrate Scalar-specific metadata into their API documentation by simply adding the appropriate attributes to their API endpoints.
/// </summary>
public class ScalarSwashbuckleOperationFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to enrich the OpenAPI operation based on the presence of Scalar-specific attributes on the API endpoint. The filter checks for <see cref="ScalarOperationAttribute"/>, <see cref="ScalarExcludeAttribute"/>, and <see cref="ScalarCodeSampleAttribute"/> on the method metadata, updating the operation's summary, description, operation ID, tags, and extensions accordingly. This allows for seamless integration of Scalar-specific metadata into the API documentation, enhancing the clarity and usability of the generated OpenAPI specifications for clients consuming the API through the Scalar UI.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 1. Handle ScalarOperationAttribute
        new OperationProcessor().ProcessAsync(operation, context);

        // 2. Handle ScalarExcludeAttribute
        new ExclusionProcessor().ProcessAsync(operation, context);

        // 3. Handle ScalarCodeSampleAttribute
        new CodeSampleProcessor().ProcessAsync(operation, context);
    }
}
#endif