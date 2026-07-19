using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

namespace Scalar.Annotation.Test.Api;

[ScalarEnum("Measurements", "Weather Messaurements", ["Celcius", "Far", "Under_Scored"])]
public enum Measurement
{
    Celcius,
    Farenhiet,
    Under_Score
}
