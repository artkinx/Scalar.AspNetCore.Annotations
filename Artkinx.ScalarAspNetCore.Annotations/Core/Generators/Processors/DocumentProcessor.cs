using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using System.Text.Json.Nodes;





#if NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
#elif NET10_0
using Microsoft.OpenApi;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;

/// <summary>
/// Enables the implementation of document transformation and filtration 
/// </summary>
public class DocumentProcessor
{
#if NET9_0_OR_GREATER
    /// <summary>
    /// The native OpenApi implementation for Document Transformation
    /// </summary>
    public virtual Task ProcessAsync(OpenApiDocument openAPiDoc, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        try
        {
#if NET9_0
            openAPiDoc.Tags ??= [];
#elif NET10_0
            openAPiDoc.Tags ??= new HashSet<OpenApiTag>();
#endif
            // Get all the discovered Apis with the ScalarOperationAttribute
            var apis = context.DescriptionGroups.SelectMany(sl => sl.Items).TakeWhile(t => t.CustomAttributes().OfType<ScalarOperationAttribute>().Any());

            foreach (var apiDescription in apis)
            {
                var scalarOp = apiDescription.ActionDescriptor.EndpointMetadata.OfType<ScalarOperationAttribute>().FirstOrDefault();

                if (scalarOp != null && !string.IsNullOrEmpty(scalarOp.DisplayName) && scalarOp.Tags != null)
                {
                    // Check if the tag is added already
                    var existingTag = openAPiDoc.Tags.FirstOrDefault(a => scalarOp.Tags?.Contains(a.Name) ?? false);
                    if (existingTag == null)
                    {
                        var newTag = new OpenApiTag { Name = scalarOp.Tags[0] };

#if NET10_0
                        newTag.AddExtension("x-displayName", new JsonNodeExtension(JsonValue.Create(scalarOp.DisplayName)));
#endif

                    }
                }
            }


        }
        catch (Exception e)
        {

        }

        return Task.CompletedTask;
    }
#endif

    /// <summary>
    /// The Swagger gen implementation for Document Filtering
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public virtual Task ProcessAsync(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        try
        {
#if NET9_0
            swaggerDoc.Tags ??= [];
#elif NET10_0
            swaggerDoc.Tags ??= new HashSet<OpenApiTag>();
#endif
            // Get all the discovered Apis with the ScalarOperationAttribute
            var apis = context.ApiDescriptions.TakeWhile(t => t.CustomAttributes().OfType<ScalarOperationAttribute>().Any());
            Console.WriteLine($"The found apis are {apis.Count()} and names: {String.Join(",", apis.Select(sl => sl.ActionDescriptor.DisplayName))}");
            foreach (var apiDescription in apis)
            {
                var scalarOp = apiDescription.ActionDescriptor.EndpointMetadata.OfType<ScalarOperationAttribute>().FirstOrDefault();

                if (scalarOp != null && !string.IsNullOrEmpty(scalarOp.DisplayName) && scalarOp.Tags != null)
                {
                    Console.WriteLine("Entered");
                    // Check if the tag is added already
                    var existingTag = swaggerDoc.Tags.FirstOrDefault(a => scalarOp.Tags?.Contains(a.Name) ?? false);
                    Console.WriteLine($"Is {existingTag == null}");
                    if (existingTag == null)
                    {
                        var newTag = new OpenApiTag { Name = scalarOp.Tags[0] };

#if NET10_0
                        newTag.AddExtension("x-displayName", new JsonNodeExtension(JsonValue.Create(scalarOp.DisplayName)));
#endif

                        swaggerDoc.Tags.Add(newTag);
                    }
                }
            }
        }
        catch (Exception e)
        {

        }

        return Task.CompletedTask;

    }

}
