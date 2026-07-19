using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

namespace Scalar.Annotation.Test.Api;

[ScalarEnum("Seasons", TransformProperties = true)]
public enum Season
{
    [ScalarEnum("Winter", prop: "The first season of the year", TransformProperties = true)]
    Winter,
    Summer,
    Harmattan
}
