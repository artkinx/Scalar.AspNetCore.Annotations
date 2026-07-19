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

        public ScalarOperationAttribute() { }

        public ScalarOperationAttribute(string summary, string? description = null, string? operationId = null, string[]? tags = null, string? themeColor = null, string? displayName = null)
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
