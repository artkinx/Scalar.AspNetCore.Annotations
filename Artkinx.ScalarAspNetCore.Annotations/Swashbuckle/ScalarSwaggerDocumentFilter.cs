using Swashbuckle.AspNetCore.SwaggerGen;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;



#if NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#endif


namespace Artkinx.ScalarAspNetCore.Annotations.Swashbuckle;

/// <summary>
/// 
/// </summary>
public class ScalarSwaggerDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        Console.WriteLine("Started Document Processing");
        new DocumentProcessor().ProcessAsync(swaggerDoc, context);
    }

}
