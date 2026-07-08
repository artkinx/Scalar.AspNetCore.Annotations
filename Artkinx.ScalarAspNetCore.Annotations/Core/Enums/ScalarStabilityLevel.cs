namespace Artkinx.ScalarAspNetCore.Annotations.Core.Enums;

/// <summary>
/// The stability level of the API operation.
/// </summary>
public enum ScalarStabilityLevel
{
    /// <summary>
    /// The API operation is stable and will not be changed in breaking ways.
    /// </summary>
    Stable,
    /// <summary>
    /// The API operation is deprecated and will be removed in a future version.
    /// </summary>
    Deprecated,
    /// <summary>
    /// The API operation is experimental and may be changed in breaking ways.
    /// </summary>
    Experimental
}