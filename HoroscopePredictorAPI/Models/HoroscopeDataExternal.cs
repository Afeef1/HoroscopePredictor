using System.Text.Json.Serialization;

namespace HoroscopePredictorAPI.Models
{
    public class HoroscopeDataExternal
    {
        [JsonPropertyName("sun_sign")]
        public string? SunSign { get; set; }

        [JsonPropertyName("prediction_date")]
        public string? PredictionDate { get; set; }

        [JsonPropertyName("prediction")]
        public PredictionDataExternal? Prediction { get; set; }
    }
}
