#if NET9_0_OR_GREATER


using Microsoft.AspNetCore.OpenApi;
#if NET9_0
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#endif
using System.Reflection;
using System.Text.Json.Nodes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;

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

            return new SchemaProcessor().ProcessAsync(schema, context, cancellationToken);
        }
    }
}
#endif