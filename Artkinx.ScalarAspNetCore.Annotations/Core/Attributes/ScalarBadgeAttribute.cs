using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Attributes
{
    /// <summary>
    /// Represents an attribute for adding badges to API endpoints in the Scalar UI.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ScalarBadgeAttribute : Attribute
    {
        /// <summary>
        /// Specifies the text to be displayed on the badge in the Scalar UI. The badge name serves as a label or identifier for the badge, providing users with a clear and concise description of the badge's purpose or significance in the context of the API documentation. By setting the name property, you can effectively communicate important information or categorization about specific API endpoints, enhancing the overall clarity and usability of your API documentation in the Scalar UI.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Specifies the position of the badge in the Scalar UI. The badge can be positioned either before or after the operation header, allowing for flexible placement based on the desired visual hierarchy and emphasis. By default, the badge is positioned after the operation header, but you can choose to position it before if you want to give it more prominence or make it stand out more in the UI. This property allows you to control the layout and presentation of badges in the Scalar UI, enhancing the visual organization and user experience of your API documentation.
        /// </summary>
        public BadgePosition Position { get; }

        /// <summary>
        /// Specifies the color of the badge in the Scalar UI. The color can be defined using a custom hex color code (e.g., "#FF5733") or a predefined theme string recognized by Scalar (e.g., "primary", "secondary", "success", "danger", etc.). By setting the color property, you can visually differentiate badges based on their significance, category, or any other criteria you choose, making it easier for users to quickly identify and understand the context of the badge in the API documentation. This customization option allows you to enhance the visual appeal and clarity of your API documentation in the Scalar UI.
        /// </summary>
        public string Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarBadgeAttribute"/> class with the specified badge name, position, and color. This constructor allows you to annotate an API endpoint with a custom badge that can be displayed in the Scalar UI, providing additional visual cues or information about the endpoint. The badge name is a required parameter that specifies the text to be displayed on the badge, while the position and color parameters are optional and allow you to customize the appearance and placement of the badge in the UI. By using this attribute, you can enhance the visibility and context of specific API endpoints in the generated OpenAPI documentation for Scalar-based APIs.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        public ScalarBadgeAttribute(string name, BadgePosition position = BadgePosition.After, string color = "default")
        {
            Name = name;
            Position = position;
            Color = color;
        }
    }
}

/// <summary>
/// Defines the possible positions for badges in the Scalar UI, allowing developers to specify whether a badge should be displayed before or after the operation header in the generated API documentation. This enumeration provides a clear and standardized way to control the placement of badges, enhancing the visual organization and user experience of API documentation in the Scalar UI.
/// </summary>
public enum BadgePosition
{
    /// <summary>
    /// Position the badge after the operation header (default).
    /// </summary>
    [Description("after")]
    After,

    /// <summary>
    /// Position the badge before the operation header.
    /// </summary>
    [Description("before")]
    Before
}