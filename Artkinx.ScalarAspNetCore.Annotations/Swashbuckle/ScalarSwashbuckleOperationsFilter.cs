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
using Artkinx.ScalarAspNetCore.Annotations.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;


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
        var scalarOperation = context.MethodInfo.GetCustomAttribute<ScalarOperationAttribute>();
        if (scalarOperation != null)
        {
            if (!string.IsNullOrEmpty(scalarOperation.Summary))
                operation.Summary = scalarOperation.Summary;

            if (!string.IsNullOrEmpty(scalarOperation.Description))
                operation.Description = scalarOperation.Description;

            if (!string.IsNullOrEmpty(scalarOperation.OperationId))
                operation.OperationId = scalarOperation.OperationId;

            if (scalarOperation.Tags != null && scalarOperation.Tags.Any())
            {
#if NET9_0
                operation.Tags = scalarOperation.Tags.Select(t => new OpenApiTag { Name = t }).ToList();
#elif NET10_0
                operation.Tags = (ISet<OpenApiTagReference>?)scalarOperation.Tags.Select(selector: t => new OpenApiTagReference(referenceId: t)).ToList();
#endif
            }

            if (!string.IsNullOrEmpty(scalarOperation.ThemeColor))
            {
#if NET9_0
                operation.Extensions["x-scalar-color"] = new OpenApiString(scalarOperation.ThemeColor);
#elif NET10_0
                operation.Extensions?["x-scalar-color"] = new JsonNodeExtension(JsonValue.Create(scalarOperation.ThemeColor));
#endif
            }
        }

        // 2. Handle ScalarExcludeAttribute
        var excludeAttribute = context.MethodInfo.GetCustomAttribute<ScalarExcludeAttribute>();
        if (excludeAttribute != null)
        {
#if NET9_0
            operation.Extensions["x-scalar-ignore"] = new OpenApiBoolean(true);
#elif NET10_0
               operation.Extensions?["x-scalar-ignore"] = new JsonNodeExtension(JsonValue.Create(true));
#endif
        }

        // 3. Handle ScalarCodeSampleAttribute
        var codeSamples = context.MethodInfo.GetCustomAttributes<ScalarCodeSampleAttribute>().ToList();
        if (codeSamples.Count > 0)
        {
#if NET9_0
            var sampleArray = new OpenApiArray();
            foreach (var sample in codeSamples)
            {
                sampleArray.Add(new OpenApiObject
                {
                    ["lang"] = new OpenApiString(sample.Language),
                    ["source"] = new OpenApiString(sample.Code),
                    ["label"] = new OpenApiString(string.IsNullOrEmpty(sample.Title) ? sample.Language : sample.Title)
                });
            }
            operation.Extensions["x-codeSamples"] = sampleArray;
#elif NET10_0
            var sampleArray = new List<dynamic>();

            foreach (var sample in codeSamples)
            {
               sampleArray.Add(new Dictionary<string, dynamic>
               {
                    ["lang"] =  new JsonNodeExtension(JsonValue.Create(sample.Language)),
                    ["source"] =  new JsonNodeExtension(JsonValue.Create(sample.Code)),
                    ["label"] =  new JsonNodeExtension(JsonValue.Create(sample.Title)),
               });
            }
#endif
        }
    }
}
#endif