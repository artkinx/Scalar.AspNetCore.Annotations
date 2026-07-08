using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Artkinx.ScalarAspNetCore.Annotations.Core.Enums;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes
{
    /// <summary>
    /// Marker attribute to indicate that the response of an API endpoint should be treated as a "Scalar response" in the context of API documentation generation. This attribute can be used by tools like Swashbuckle to apply specific transformations or enhancements to the OpenAPI documentation for endpoints that return Scalar responses, such as adding custom extensions, modifying schema representations, or applying special formatting rules.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ScalarResponseAttribute : ProducesResponseTypeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarResponseAttribute"/> class with the specified HTTP status code and an optional description. This constructor allows you to annotate an API endpoint with a specific response status code and provide additional context or information about the response using the description parameter, which can be utilized by API documentation generators to enhance the generated OpenAPI documentation for endpoints that return Scalar responses.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="description"></param>
        public ScalarResponseAttribute(int statusCode, string description = "") : base(statusCode)
        {
            Description = description;
            ContentTypes = Array.Empty<string>();
        }

        /// <summary>
        ///    Initializes a new instance of the <see cref="ScalarResponseAttribute"/> class with the specified response type and HTTP status code. This constructor allows you to annotate an API endpoint with a specific response type (e.g., a model class) and an associated HTTP status code, indicating that the endpoint returns a Scalar response of the specified type. This information can be leveraged by API documentation generators to enhance the OpenAPI documentation for endpoints that return Scalar responses, providing more detailed information about the expected response schema and status code.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="statusCode"></param>
        /// <param name="additionalContentTypes"></param>
        public ScalarResponseAttribute(Type type, int statusCode, params ScalarContentType[] additionalContentTypes) : base(type, statusCode)
        {
            ContentTypes = additionalContentTypes?.Select(c => c.ToMimeString()).ToArray() ?? Array.Empty<string>();
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarResponseAttribute"/> class with the specified response type, HTTP status code, primary content type, and additional content types. This constructor allows you to annotate an API endpoint with a specific response type, an associated HTTP status code, and multiple content types that the endpoint can produce in its response. This information can be utilized by API documentation generators to enhance the OpenAPI documentation for endpoints that return Scalar responses by providing detailed information about the expected response schema, status code, and the various formats in which the response can be provided.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="statusCode"></param>
        /// <param name="contentType"></param>
        /// <param name="description"></param>
        /// <param name="additionalContentTypes"></param>
        public ScalarResponseAttribute(Type type, int statusCode, ScalarContentType contentType, string description = "", params ScalarContentType[] additionalContentTypes) : base(type, statusCode, contentType.ToMimeString(), additionalContentTypes?.Select(c => c.ToMimeString()).ToArray() ?? Array.Empty<string>())
        {
            ContentTypes = (new[] { contentType.ToMimeString() }).Concat(additionalContentTypes?.Select(c => c.ToMimeString()) ?? Array.Empty<string>()).ToArray();
            Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarResponseAttribute"/> class with the specified HTTP status code. This constructor allows you to annotate an API endpoint with a specific response status code, indicating that the endpoint returns a Scalar response with that status code. This information can be leveraged by API documentation generators to enhance the OpenAPI documentation for endpoints that return Scalar responses by providing information about the expected response status code.
        /// </summary>
        public ScalarResponseAttribute() : base(200) // Default to 200 OK if no status code is provided
        {
            Description = string.Empty;
            ContentTypes = Array.Empty<string>();
        }

#if !NET10_0_OR_GREATER
        /// <summary>
        /// Gets or sets a description for the response, providing additional context or information about the response that can be utilized by API documentation generators to enhance the generated OpenAPI documentation for endpoints that return Scalar responses. This property allows you to provide a textual description of the response, which can help clarify the purpose or meaning of the response in the context of the API documentation.
        /// </summary>
        public string Description { get; set; }
#endif
        /// <summary>
        /// Gets or sets an array of additional content types that the API endpoint can produce in its response. This property allows you to specify multiple content types (e.g., "application/json", "application/xml") that the endpoint can return, which can be utilized by API documentation generators to enhance the OpenAPI documentation for endpoints that return Scalar responses by indicating the various formats in which the response can be provided.
        /// </summary>
        public String[] ContentTypes { get; set; }

    }
}
