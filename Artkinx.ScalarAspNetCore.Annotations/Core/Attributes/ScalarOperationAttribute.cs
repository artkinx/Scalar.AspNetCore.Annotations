using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes
{
    /// <summary>
    /// Enriches the OpenAPI Operation with Scalar-specific UI metadata.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class ScalarOperationAttribute : Attribute
    {
        /// <summary>
        /// A short summary of what the operation does.
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the operation behavior.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The unique string used to identify the operation.
        /// </summary>
        public string? OperationId { get; set; }

        /// <summary>
        /// Overrides the default tags for grouping in the Scalar UI.
        /// </summary>
        public string[]? Tags { get; set; }

        /// <summary>
        /// A custom hex color or theme string applied to this specific endpoint in the Scalar UI.
        /// </summary>
        public string? ThemeColor { get; set; }

        /// <summary>
        /// A custom operation display name
        /// </summary>
        /// <returns></returns>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarOperationAttribute"/> class.
        /// </summary>
        public ScalarOperationAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarOperationAttribute"/> class.
        /// </summary>
        /// <param name="summary">A short summary of what the operation does.</param>
        /// <param name="description">A verbose explanation of the operation behavior.</param>
        /// <param name="operationId">The unique string used to identify the operation.</param>
        /// <param name="tags">Overrides the default tags for grouping in the Scalar UI.</param>
        /// <param name="themeColor">A custom hex color or theme string applied to this specific endpoint in the Scalar UI.</param>
        /// <param name="displayName">A custom operation display name that overrides tag name if the <see cref="Tags"/> is set.</param>
        public ScalarOperationAttribute(
            string summary,
            string? description = null,
            string? operationId = null,
            string[]? tags = null,
            string? themeColor = null,
            string? displayName = null)
        {
            Summary = summary;
            Description = description;
            OperationId = operationId;
            Tags = tags;
            ThemeColor = themeColor;
            DisplayName = displayName;
        }
    }
}
