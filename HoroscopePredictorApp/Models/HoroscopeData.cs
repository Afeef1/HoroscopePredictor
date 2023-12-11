using System.Text.Json.Serialization;

namespace HoroscopePredictorApp.Models
{
    public class HoroscopeData
    {
        [JsonPropertyName("sun_sign")]
        public string? SunSign { get; set; }

        [JsonPropertyName("prediction_date")]
        public string? PredictionDate { get; set; }

        [JsonPropertyName("prediction")]
        public PredictionData? Prediction { get; set; }
    }
}
