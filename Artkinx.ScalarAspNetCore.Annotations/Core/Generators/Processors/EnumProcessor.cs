using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.OpenApi;
using Artkinx.ScalarAspNetCore.Annotations.Native;
using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;
using System.Text;
using System.Text.Json.Nodes;
using System.Reflection;


#if NET10_0
using Microsoft.OpenApi;

#elif NET9_0 || NET8_0
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Interfaces;
#endif

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Generators.Processors;

internal class EnumProcessor : IAttributeProcessor
{

#if NET9_0_OR_GREATER
    public override Task ProcessAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public override Task ProcessAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var type = context.JsonTypeInfo.Type;
            Console.WriteLine($"Processing type {type.FullName} for Enums");

            if (!type.IsEnum)
            {
                return Task.CompletedTask;
            }
#if NET9_0
            schema.Extensions ??= new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
#elif NET10_0
            schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
#endif
            // check top-level declaration
            var attrs = type.GetCustomAttributes(false);

            if (attrs.Any(a => a is ScalarEnumAttribute))
            {
                var attr = attrs.FirstOrDefault(w => w is ScalarEnumAttribute) as ScalarEnumAttribute;

                if (attr?.PropNames == null)
                {
                    Console.WriteLine("For top-level class definitions, [PropNames] must be defined");
                    return Task.CompletedTask;
                }

                // just assign the propNames to the extension
#if NET9_0
                var enumArray = new OpenApiArray();
                foreach (var item in attr.PropNames)
                {
                    enumArray.Add(new OpenApiString(FormatString(item)));
                }
#elif NET10_0
                var enumArray = new JsonArray();
                foreach (var item in attr.PropNames)
                {
                    enumArray.Add(JsonValue.Create(FormatString(item)));
                }

                schema.AddExtension("x-enum-varnames", new JsonNodeExtension(enumArray));
#endif
                return Task.CompletedTask;
            }

            // Handles property level attribute definitions
            var props = type.GetMembers();

            if (props.Any(a => a.GetCustomAttribute<ScalarEnumAttribute>() != null))
            {
                var names = Enum.GetNames(type);
                var attr = props.Where(w => w.GetCustomAttribute<ScalarEnumAttribute>(false) != null).Select(sl => (sl.Name, sl.GetCustomAttribute<ScalarEnumAttribute>(false))).ToDictionary();
                var checkany = attr.Any(a => a.Value?.TransformProperties is true);
                if (checkany)
                    schema.Enum = [];
#if NET10_0
                if (checkany)
                {
                    schema.Type = JsonSchemaType.String;
                    schema.Format = null;
                }



                var enumArray = new JsonObject();
#elif NET9_0 || NET8_0
                if (checkany)
                {
                    schema.Type = "string";
                    schema.Format = null;
                }
                var enumArray = new OpenApiObject();
#endif
                foreach (var item in names)
                {
                    var enumScalarAttr = new ScalarEnumAttribute("");
                    var check = attr.TryGetValue(item, out enumScalarAttr);
                    if (check && enumScalarAttr != null)
                    {
#if NET10_0
                        if (checkany)
                            schema.Enum?.Add(JsonValue.Create(item));

                        if (enumScalarAttr.PropName == null)
                        {
                            enumArray.Add(item, JsonValue.Create(FormatString(item)));
                            continue;
                        }
                        enumArray.Add(item, JsonValue.Create(FormatString(enumScalarAttr.PropName)));
#elif NET9_0 || NET8_0
                        if (checkany)
                            schema.Enum?.Add(new OpenApiString(item));

                        if (enumScalarAttr.PropName == null)
                        {
                            enumArray.Add(item, new OpenApiString(FormatString(item)));
                            continue;
                        }
                        enumArray.Add(item, new OpenApiString(FormatString(enumScalarAttr.PropName)));
#endif
                    }
                    else
                    {
#if NET10_0
                        if (checkany)
                            schema.Enum?.Add(JsonValue.Create(item));

                        enumArray.Add(item, JsonValue.Create(FormatString(item)));
#elif NET9_0 || NET8_0
                        if (checkany)
                            schema.Enum?.Add(new OpenApiString(item));

                        enumArray.Add(item, new OpenApiString(FormatString(item)));
#endif
                    }
                }
#if NET10_0
                schema.AddExtension("x-enum-descriptions", new JsonNodeExtension(enumArray));
#elif NET9_0 || NET8_0
                schema.AddExtension("x-enum-descriptions", enumArray);
#endif
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error processing enum: {e.Message}");
        }

        return Task.CompletedTask;
    }
#endif

    public override Task ProcessAsync(OpenApiOperation operation, OperationFilterContext context, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task ProcessAsync(OpenApiSchema schema, SchemaFilterContext context, CancellationToken cancellationToken = default)
    {
        var type = context.Type;
        Console.WriteLine($"Processing type {type.FullName} for Enums");

        if (!type.IsEnum)
        {
            return Task.CompletedTask;
        }
#if NET9_0 || NET8_0
        schema.Extensions ??= new Dictionary<string, Microsoft.OpenApi.Interfaces.IOpenApiExtension>();
#elif NET10_0
        schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
#endif
        // check top-level declaration
        var attrs = type.GetCustomAttributes(false);

        if (attrs.Any(a => a is ScalarEnumAttribute && a is ScalarEnumAttribute { PropNames: not null }))
        {
            var attr = attrs.FirstOrDefault(w => w is ScalarEnumAttribute) as ScalarEnumAttribute;
            if (attr.TransformProperties)
            {
                schema.Enum = [];
            }

            if (attr?.PropNames == null)
            {
                Console.WriteLine("For top-level class definitions, [PropNames] must be defined");
                return Task.CompletedTask;
            }

            // just assign the propNames to the extension
#if NET9_0 || NET8_0
            var enumArray = new OpenApiArray();
            foreach (var item in attr.PropNames)
            {
                if (attr.TransformProperties)
                    schema.Enum.Add(new OpenApiString(item));

                enumArray.Add(new OpenApiString(FormatString(item)));
            }
            schema.AddExtension("x-enum-varnames", enumArray);
#elif NET10_0
            var enumArray = new JsonArray();
            foreach (var item in attr.PropNames)
            {
                if (attr.TransformProperties)
                    schema.Enum?.Add(JsonValue.Create(item));

                enumArray.Add(JsonValue.Create(FormatString(item)));
            }

            schema.AddExtension("x-enum-varnames", new JsonNodeExtension(enumArray));
#endif
            return Task.CompletedTask;
        }


        // Handles property level attribute definitions

        // Retrieve the enum memebers
        var props = type.GetMembers();

        // check for the scalar custom attribute
        if (props.Any(a => a.GetCustomAttribute<ScalarEnumAttribute>() != null))
        {
            // get the enum names and properties
            var names = Enum.GetNames(type);
            var attr = props.Where(w => w.GetCustomAttribute<ScalarEnumAttribute>(false) != null).Select(sl => (sl.Name, sl.GetCustomAttribute<ScalarEnumAttribute>(false))).ToDictionary();
            var checkany = attr.Any(a => a.Value?.TransformProperties is true);

            // check if the transform flag is set to true and clear the enum values
            if (checkany)
                schema.Enum = [];

#if NET10_0
            // check if the transform flag is true and reset the schema type and format
            if (checkany)
            {
                schema.Type = JsonSchemaType.String;
                schema.Format = null;
            }
            // Prepare the enum object
            var enumArray = new JsonObject();
#elif NET9_0 || NET8_0
            // check if the transform flag is true and reset the schema type and format
            if (checkany)
            {
                schema.Type = "string";
                schema.Format = null;
            }
            // Prepare the enum object
            var enumArray = new OpenApiObject();
#endif
            // Iterate through the enum names and perform necessary operations
            foreach (var item in names)
            {
                var enumScalarAttr = new ScalarEnumAttribute("");
                var check = attr.TryGetValue(item, out enumScalarAttr);
                if (check && enumScalarAttr != null)
                {
#if NET10_0
                    // if the transform flag is true then add the property string name entry to the schema
                    if (checkany)
                        schema.Enum?.Add(JsonValue.Create(item));

                    // if the propname is not provided then repeat the enum string as key and value
                    if (enumScalarAttr.PropName == null)
                    {
                        enumArray.Add(item, JsonValue.Create(FormatString(item)));
                        continue;
                    }
                    // else add the enum string as key and then format the propname as the value
                    enumArray.Add(item, JsonValue.Create(FormatString(enumScalarAttr.PropName)));
#elif NET9_0 || NET8_0
                    if (checkany)
                        schema.Enum?.Add(new OpenApiString(item));

                    if (enumScalarAttr.PropName == null)
                    {
                        enumArray.Add(item, new OpenApiString(FormatString(item)));
                        continue;
                    }
                    enumArray.Add(item, new OpenApiString(FormatString(enumScalarAttr.PropName)));
#endif
                }
                else
                {
#if NET10_0
                    if (checkany)
                        schema.Enum?.Add(JsonValue.Create(item));

                    enumArray.Add(item, JsonValue.Create(FormatString(item)));
#elif NET9_0 || NET8_0
                    if (checkany)
                        schema.Enum?.Add(new OpenApiString(item));

                    enumArray.Add(item, new OpenApiString(FormatString(item)));
#endif
                }
            }
#if NET10_0
            schema.AddExtension("x-enum-descriptions", new JsonNodeExtension(enumArray));
#elif NET9_0 || NET8_0
            schema.AddExtension("x-enum-descriptions", enumArray);
#endif
        }

        return Task.CompletedTask;
    }


    internal static string FormatString(string enumValue)
    {
        if (enumValue.Contains('_'))
        {
            var strs = enumValue.Split('_');

            // capitalize and join
            var val = CapitalizeString(string.Join(' ', strs), " ");

            return val;
        }

        // check if already spearated by space
        if (enumValue.Trim().Contains(' '))
        {
            return CapitalizeString(enumValue, " ");
        }
        return CapitalizeString(enumValue, "");
    }

    internal static string CapitalizeString(string value, string separator)
    {
        var str = value.Split(' ').Select(sl =>
        {
            var val = sl[0];
            var newval = sl[1..];
            var d = newval.Prepend(val.ToString().ToUpper()[0]);
            return string.Join("", d);
        });

        return string.Join(separator, str);
    }
}
