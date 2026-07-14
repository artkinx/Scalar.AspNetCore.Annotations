#if NET8_0_OR_GREATER
using Artkinx.ScalarAspNetCore.Annotations.Native;


using Microsoft.OpenApi;
#if NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if NET10_0
using System.Text.Json.Nodes;
#endif
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators
{
    internal class ApiSchemaGenerator
    {
        public static OpenApiSchema GenerateSchemaFromType(Type targetType)
        {
#if NET9_0
            if (targetType == null) return new OpenApiSchema { Type = "null" };
            // Create a dummy instance or default value for the given Type
            object? instance = Converters.CreateInstance(targetType);
            if (instance == null) return new OpenApiSchema { Type = "null" };
#elif NET10_0
            if (targetType == null) return new OpenApiSchema { Type = JsonSchemaType.Null };
            // Create a dummy instance or default value for the given Type
            object? instance = Converters.CreateInstance(targetType);
            if (instance == null) return new OpenApiSchema { Type = JsonSchemaType.Null };
#endif
            // Generate schema based on the type of the instance
            var schema = new OpenApiSchema();
            if (targetType.IsPrimitive || targetType == typeof(string))
            {
#if NET9_0
                schema.Type = targetType.Name.ToLowerInvariant();
#elif NET10_0
                schema.Type = JsonSchemaType.String;
#endif
            }
            else if (targetType.IsEnum)
            {
#if NET9_0
                schema.Type = "string";
                schema.Enum = (IList<Microsoft.OpenApi.Any.IOpenApiAny>)Enum.GetNames(targetType).ToList();
#elif NET10_0
                schema.Type = JsonSchemaType.String;
                schema.Enum = [.. Enum.GetNames(targetType).Select(v => JsonNode.Parse(JsonValue.Create(v).ToString()))];
#endif
            }
            else if (typeof(IEnumerable<>).IsAssignableFrom(targetType))
            {
#if NET9_0
                schema.Type = "array";
                var elementType = targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                schema.Items = GenerateSchemaFromType(elementType);
#elif NET10_0
                schema.Type = JsonSchemaType.Array;
                var elementType = targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);
                schema.Items = GenerateSchemaFromType(elementType);
#endif
            }
            else
            {
#if NET9_0
                schema.Type = "object";
                schema.Properties = new Dictionary<string, OpenApiSchema>();
                foreach (var property in targetType.GetProperties())
                {
                    schema.Properties[property.Name] = GenerateSchemaFromType(property.PropertyType);
                }
#elif NET10_0
                schema.Type = JsonSchemaType.Object;
                schema.Properties = new Dictionary<string, IOpenApiSchema>();
                foreach (var property in targetType.GetProperties())
                {
                    schema.Properties[property.Name] = GenerateSchemaFromType(property.PropertyType);
                }
#endif
            }
            return schema;
        }

       
    }
}
#endif