using Artkinx.ScalarAspNetCore.Annotations.Core.Attributes;

namespace Scalar.Annotation.Test.Api
{
    public class WeatherForecast
    {
        [ScalarOrder(4)]
        public DateOnly Date { get; set; }

        [ScalarOrder(1)]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}
