using System.ComponentModel;

namespace Artkinx.ScalarAspNetCore.Annotations.Core.Enums
{
    /// <summary>
    /// Represents standard content types used in HTTP APIs.
    /// </summary>
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public enum ScalarContentType
    {
        [Description("application/json")]
        Json,
        
        [Description("application/xml")]
        Xml,
        
        [Description("text/plain")]
        TextPlain,
        
        [Description("text/html")]
        TextHtml,
        
        [Description("application/x-www-form-urlencoded")]
        FormUrlEncoded,
        
        [Description("multipart/form-data")]
        MultipartFormData,
        
        [Description("application/octet-stream")]
        OctetStream,
        
        [Description("application/pdf")]
        Pdf,
        
        [Description("text/csv")]
        Csv,
        
        [Description("image/png")]
        ImagePng,
        
        [Description("image/jpeg")]
        ImageJpeg,
        
        [Description("image/gif")]
        ImageGif,
        
        [Description("image/webp")]
        ImageWebp,

        [Description("application/bson")]
        Bson,

        [Description("application/x-yaml")]
        Yaml,

        [Description("application/x-msgpack")]
        MessagePack,

        [Description("application/graphql+json")]
        GraphQl,

        [Description("text/event-stream")]
        EventStream,

        [Description("application/problem+json")]
        ProblemJson,

        [Description("application/problem+xml")]
        ProblemXml,

        [Description("application/ld+json")]
        JsonLd
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    internal static class ScalarContentTypeExtensions
    {
        public static string ToMimeString(this ScalarContentType contentType)
        {
            return contentType switch
            {
                ScalarContentType.Json => "application/json",
                ScalarContentType.Xml => "application/xml",
                ScalarContentType.TextPlain => "text/plain",
                ScalarContentType.TextHtml => "text/html",
                ScalarContentType.FormUrlEncoded => "application/x-www-form-urlencoded",
                ScalarContentType.MultipartFormData => "multipart/form-data",
                ScalarContentType.OctetStream => "application/octet-stream",
                ScalarContentType.Pdf => "application/pdf",
                ScalarContentType.Csv => "text/csv",
                ScalarContentType.ImagePng => "image/png",
                ScalarContentType.ImageJpeg => "image/jpeg",
                ScalarContentType.ImageGif => "image/gif",
                ScalarContentType.ImageWebp => "image/webp",
                ScalarContentType.Bson => "application/bson",
                ScalarContentType.Yaml => "application/x-yaml",
                ScalarContentType.MessagePack => "application/x-msgpack",
                ScalarContentType.GraphQl => "application/graphql+json",
                ScalarContentType.EventStream => "text/event-stream",
                ScalarContentType.ProblemJson => "application/problem+json",
                ScalarContentType.ProblemXml => "application/problem+xml",
                ScalarContentType.JsonLd => "application/ld+json",
                _ => "application/json"
            };
        }
    }
}
