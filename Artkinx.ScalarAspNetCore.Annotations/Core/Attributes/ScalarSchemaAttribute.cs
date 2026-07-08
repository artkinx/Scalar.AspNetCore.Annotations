using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes
{
    /// <summary>
    /// Enriches the OpenAPI Schema for requests/responses with mock data and descriptions.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field,
        Inherited = false,
        AllowMultiple = false)]
    public class ScalarSchemaAttribute : Attribute
    {
        /// <summary>
        /// A description of the property or class.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// A specific format for the string (e.g., "date-time", "uuid", "email").
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// A realistic mock value used by the Scalar UI API client.
        /// </summary>
        public object? MockValue { get; set; }

        /// <summary>
        /// Marks the property as read-only (not expected in requests).
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Marks the property as write-only (hidden in responses, e.g., passwords).
        /// </summary>
        public bool WriteOnly { get; set; }

        public ScalarSchemaAttribute() { }

        public ScalarSchemaAttribute(string description)
        {
            Description = description;
        }
    }
}
