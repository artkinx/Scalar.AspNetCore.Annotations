using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
#endif


namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors
{

    internal abstract class IAttributeProcessor
    {
#if NET9_0_OR_GREATER
        /// <summary>
        /// Native implementation that Processes the given OpenAPI operation by inspecting its metadata and applying any relevant transformations or enhancements based on Scalar-specific attributes. This method is intended to be called during the OpenAPI generation process, allowing for the injection of additional information, such as descriptions, examples, or custom extensions, into the generated documentation for operations that are decorated with Scalar attributes.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task ProcessAsync(
                 OpenApiOperation operation,
                 OpenApiOperationTransformerContext context,
                 CancellationToken cancellationToken = default);
        /// <summary>
        /// Native implementation that Processes the given OpenAPI operation by inspecting its metadata and applying any relevant transformations or enhancements based on Scalar-specific attributes. This method is intended to be called during the OpenAPI generation process, allowing for the injection of additional information, such as descriptions, examples, or custom extensions, into the generated documentation for operations that are decorated with Scalar attributes.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task ProcessAsync(
                 OpenApiSchema schema,
                 OpenApiSchemaTransformerContext context,
                 CancellationToken cancellationToken = default);
#endif
        /// <summary>
        /// Swashbuckle implementation that Processes the given OpenAPI operation by inspecting its metadata and applying any relevant transformations or enhancements based on Scalar-specific attributes. This method is intended to be called during the OpenAPI generation process, allowing for the injection of additional information, such as descriptions, examples, or custom extensions, into the generated documentation for operations that are decorated with Scalar attributes.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task ProcessAsync(
         OpenApiOperation operation,
         OperationFilterContext context,
         CancellationToken cancellationToken = default);

        /// <summary>
        /// Swashbuckle implementation that Processes the given OpenAPI operation by inspecting its metadata and applying any relevant transformations or enhancements based on Scalar-specific attributes. This method is intended to be called during the OpenAPI generation process, allowing for the injection of additional information, such as descriptions, examples, or custom extensions, into the generated documentation for operations that are decorated with Scalar attributes.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task ProcessAsync(
         OpenApiSchema schema,
         SchemaFilterContext context,
         CancellationToken cancellationToken = default);

    }

    internal abstract class INativeAttributeSchemaProcessor { }
}
