#if NET9_0_OR_GREATER
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;
using Microsoft.AspNetCore.OpenApi;

#if NET9_0
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Native;

/// <summary>
/// Implement a means to transform native OpenApi Documents
/// </summary>
public class ScalarNativeDocumentTransformer : IOpenApiDocumentTransformer
{
        /// <summary>
        /// Hooks up to the pipeline and runs the transformation operation
        /// </summary>
        /// <param name="document"></param>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
       return new DocumentProcessor().ProcessAsync(document, context, cancellationToken);
    }
}

#endif