namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

/// <summary>
/// Represents an attribute that can be applied to API endpoints to indicate that they should be excluded from the Scalar UI. When this attribute is present on a method, it serves as a marker to hide the corresponding endpoint from being displayed in the Scalar UI, while still keeping it accessible in the underlying OpenAPI JSON documentation. This allows developers to control the visibility of specific API endpoints in the Scalar UI without affecting their availability in the API documentation or functionality. By using this attribute, you can effectively manage which endpoints are showcased in the Scalar UI, providing a cleaner and more focused user experience for API consumers.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ScalarExcludeAttribute : Attribute
{
    // Simple marker attribute to hide an endpoint from the Scalar UI
    // while keeping it in the underlying OpenAPI JSON.
}

