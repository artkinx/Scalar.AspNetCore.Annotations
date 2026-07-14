using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes
{
    /// <summary>
    /// Indicates that the decorated property should be used to determine the order of scalar properties in the generated OpenAPI schema. This attribute is intended for use on properties within a class to specify their relative ordering when serialized or displayed in the Scalar UI. When applied, it allows developers to control the sequence of properties, enhancing the clarity and usability of the API documentation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ScalarOrderAttribute(int order) : Attribute
    {
        /// <summary>
        /// Gets the order value specified for the property. This value is used to determine the relative position of the property in the generated OpenAPI schema and in the Scalar UI. Lower values indicate higher priority, meaning properties with lower order values will appear before those with higher values.
        /// </summary>
        public int Order { get; } = order;
    }
}
