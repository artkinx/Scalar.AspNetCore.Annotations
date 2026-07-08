using Artkinx.ScalarAspNetCore.Annotations.Core.Enums;
namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

/// <summary>
/// Represents an attribute that can be applied to API endpoints to indicate their stability level. When this attribute is present on a method, it serves as a marker to indicate the stability level of the corresponding endpoint in the Scalar UI. This allows developers to control the visibility of specific API endpoints in the Scalar UI, providing a cleaner and more focused user experience for API consumers.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class ScalarStabilityAttribute(ScalarStabilityLevel level) : Attribute
{
    /// <summary>
    /// Gets the stability level of the API operation.
    /// </summary>
    public ScalarStabilityLevel Level { get; set; } = level;
}
