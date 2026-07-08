
#if NET9_0
using Microsoft.OpenApi.Any;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Native
{
#if NET9_0

    /// <summary>
    /// Builds an OpenAPI example object from a given .NET type and content type. This method generates a dummy instance of the specified type, serializes it to the appropriate format (JSON or XML), and returns it as an IOpenApiAny object that can be used in OpenAPI documentation.
    /// </summary>
    public static class OpenApiExampleBuilder
    {
        /// <summary>
        /// Builds an OpenAPI example object from a given .NET type and content type. This method generates a dummy instance of the specified type, serializes it to the appropriate format (JSON or XML), and returns it as an IOpenApiAny object that can be used in OpenAPI documentation.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static IOpenApiAny BuildExampleFromType(Type targetType, string contentType)
        {
            if (targetType == null) return new OpenApiNull();

            // 1. Create a dummy instance or default value for the given Type
            object? instance = Converters.CreateInstance(targetType);
            if (instance == null) return new OpenApiNull();

            // 2. Normalize and check the content type
            var normalizedType = contentType.Split(';')[0].Trim().ToLowerInvariant();

            switch (normalizedType)
            {
                case "application/json":
                case "text/json":
                case "application/x-www-form-urlencoded":
                case "multipart/form-data":
                    // Serialize the generated object to raw JSON
                    var jsonString = JsonSerializer.Serialize(instance, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    });

                    // .NET 9 safely maps JSON structures to explicit IOpenApiAny trees
                    using (var doc = JsonDocument.Parse(jsonString))
                    {
                        return ConvertJsonToOpenApiAny(doc.RootElement);
                    }
                case "application/xml":
                case "text/xml":
                    return new OpenApiString(Converters.ConvertToXmlString(instance));

                default:
                    return new OpenApiString(instance.ToString() ?? string.Empty);
            }
        }

        private static IOpenApiAny ConvertJsonToOpenApiAny(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => ConvertJsonObjectToOpenApiAny(element),
                JsonValueKind.Array => ConvertJsonArrayToOpenApiAny(element),
                JsonValueKind.String => new OpenApiString(element.GetString() ?? string.Empty),
                JsonValueKind.Number => new OpenApiDouble(element.GetDouble()),
                JsonValueKind.True => new OpenApiBoolean(true),
                JsonValueKind.False => new OpenApiBoolean(false),
                JsonValueKind.Null => new OpenApiNull(),
                _ => throw new NotSupportedException($"Unsupported JSON value kind: {element.ValueKind}")
            };
        }

        private static IOpenApiAny ConvertJsonArrayToOpenApiAny(JsonElement element)
        {
            var apiObject = new OpenApiObject();
            foreach (var property in element.EnumerateObject())
            {
                apiObject[property.Name] = ConvertJsonToOpenApiAny(property.Value);
            }
            return apiObject;
        }

        private static IOpenApiAny ConvertJsonObjectToOpenApiAny(JsonElement element)
        {
            var apiArray = new OpenApiArray();
            foreach (var item in element.EnumerateArray())
            {
                apiArray.Add(ConvertJsonToOpenApiAny(item));
            }
            return apiArray;
        }
    }
#elif NET10_0
    /// <summary>
    /// Builds an OpenAPI example object from a given .NET type and content type. This method generates a dummy instance of the specified type, serializes it to the appropriate format (JSON or XML), and returns it as a JsonNode that can be used in OpenAPI documentation.
    /// </summary>
    public static class OpenApiExampleBuilder
    {
        /// <summary>
        /// Builds an OpenAPI example object from a given .NET type and content type. This method generates a dummy instance of the specified type, serializes it to the appropriate format (JSON or XML), and returns it as a JsonNode that can be used in OpenAPI documentation.
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static JsonNode? BuildExampleFromType(Type targetType, string contentType)
        {
            if (targetType == null) return null;

            object? instance = Converters.CreateInstance(targetType);
            if (instance == null) return null;

            var normalizedType = contentType.Split(';')[0].Trim().ToLowerInvariant();

            switch (normalizedType)
            {
                case "application/json":
                case "text/json":
                case "application/x-www-form-urlencoded":
                case "multipart/form-data":
                    // 1. Convert the object instance directly to a structured JsonNode tree structure
                    return JsonSerializer.SerializeToNode(instance, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = false
                    });

                case "application/xml":
                case "text/xml":
                    // 2. XML is mapped as a single text block node
                    return JsonValue.Create(Converters.ConvertToXmlString(instance));

                default:
                    // 3. Fallback to primitive value node wrapping
                    return JsonValue.Create(instance.ToString() ?? string.Empty);
            }
        }

    }
#endif
}
