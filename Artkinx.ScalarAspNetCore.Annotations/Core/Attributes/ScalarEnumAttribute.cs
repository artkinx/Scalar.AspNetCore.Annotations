using System.Text.Json.Serialization;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

/// <summary>
/// Represents
/// </summary>
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class ScalarEnumAttribute(string title, string? description = null, string[]? props = null, string? prop = null, bool transformProperties = false) : JsonConverterAttribute
{
    /// <summary>
    /// Ensures that the Client uses the correct type
    /// </summary>
    /// <param name="typeToConvert"></param>
    /// <returns></returns>
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        if (TransformProperties)
            return new JsonStringEnumConverter();

        var converterType = typeof(JsonNumberEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter?)Activator.CreateInstance(converterType);
    }
    /// <summary>
    /// The title of the enum.
    /// </summary>
    public string Title { get; set; } = title;
    /// <summary>
    /// The description of the enum.
    /// </summary>
    public string? Description { get; set; } = description;
    /// <summary>
    /// A list of strings defining the enum property names. Should only be used when making use of top-level attribute declaration
    /// </summary>
    /// <value></value>
    public string[]? PropNames { get; set; } = props;
    /// <summary>
    /// The specified <see langword="PropName"/> name for the enum member
    /// </summary>
    /// <value></value>
    public string? PropName { get; set; } = prop;
    /// <summary>
    /// A flag that defines whether to transform the enum properties by using the Names and not the Values.
    /// If the <see cref="ScalarEnumAttribute"/> is placed on a property/properties, you just need to set one true to transform everything.
    /// Setting <see href="TransformProperties"/> to false would mean that the Api Client will use the number values of the Enum when serializing.
    /// Note: if you use this attribute on Enum properties alone and you want the Enum Serialization to be strings then add the [ScalarEnumAttribute] to the top of the enum class and set TransformProperties to True.
    /// </summary>
    /// <value>Defaults to false</value>
    public bool TransformProperties { get; set; } = transformProperties;



}
