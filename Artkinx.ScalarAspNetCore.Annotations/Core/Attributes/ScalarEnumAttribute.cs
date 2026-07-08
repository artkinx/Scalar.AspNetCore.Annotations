namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

/// <summary>
/// Represents
/// </summary>
[AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public class ScalarEnumAttribute(string title, string? description) : Attribute
{
    /// <summary>
    /// The title of the enum.
    /// </summary>
    public string Title { get; set; } = title;
    /// <summary>
    /// The description of the enum.
    /// </summary>
    public string? Description { get; set; } = description;
}
