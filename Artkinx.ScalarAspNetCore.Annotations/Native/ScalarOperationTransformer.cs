
#if NET9_0_OR_GREATER

using Microsoft.AspNetCore.OpenApi;
using System.Text.Json.Nodes;
using System.Text.Json;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;


#if NET9_0
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
#elif NET10_0_OR_GREATER
using Microsoft.OpenApi;
#endif


namespace Artkinx.ScalarAspNetCore.Annotations.Native;

/// <summary>
/// Transforms OpenAPI operations by inspecting endpoint metadata for Scalar-specific attributes and injecting corresponding OpenAPI extensions that Swashbuckle can utilize to enhance the generated documentation with badges, code samples, and exclusion flags.
/// </summary>
public class ScalarOperationTransformer : IOpenApiOperationTransformer
{
    /// <summary>
    /// Transforms the given OpenAPI operation by extracting Scalar-specific attributes from the endpoint metadata and adding appropriate extensions to the operation definition. This allows Swashbuckle to recognize and render badges, code samples, and exclusion flags in the generated API documentation.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        // Extract all attributes from the endpoint metadata
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;


#if NET9_0
        if (operation.Extensions == null)
        {
            operation.Extensions = new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
        }
#elif NET10_0
        if (operation.Extensions == null)
        {
            operation.Extensions = new Dictionary<string, IOpenApiExtension>();
        }
#endif


        // 1. Handle ScalarOperationAttribute for general operation metadata
        new OperationProcessor().ProcessAsync(operation, context, cancellationToken);

  // 1. Handle Badges
        new BadgeProcessor().ProcessAsync(operation, context, cancellationToken);


        // 2. Handle Code Samples
        new CodeSampleProcessor().ProcessAsync(operation, context, 
            cancellationToken);

        // 3. Handle Exclusions
        new ExclusionProcessor().ProcessAsync(operation, context, cancellationToken);

        // 4. Handle Stability
        new StabilityProcessor().ProcessAsync(operation, context, cancellationToken);

        return Task.CompletedTask;
    }



}
#endif




public static class Converters
{
    public static object? CreateInstance(Type type)
    {
        // Handle standard system primitives and strings directly
        if (type == typeof(string)) return "string";
        if (type == typeof(int) || type == typeof(long)) return 0;
        if (type == typeof(bool)) return false;
        if (type == typeof(double) || type == typeof(decimal)) return 0.0;
        if (type == typeof(Guid)) return Guid.NewGuid();
        if (type == typeof(DateTime)) return DateTime.UtcNow;

        try
        {
            // For complex custom types/DTOs, invoke the parameterless constructor
            return Activator.CreateInstance(type);
        }
        catch
        {
            // Fallback if the object lacks a default constructor (e.g., record types or custom constructors)
            // You can optionally swap this for a mock object framework or uninitialized object storage
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
        }
    }

    public static string ConvertToXmlString(object obj)
    {
        using var stringWriter = new System.IO.StringWriter();
        var serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
        serializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }
}