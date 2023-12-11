using System.Text.Json.Serialization;

namespace HoroscopePredictorApp.Models
{
    public class PredictionData
    {

        [JsonPropertyName("personal_life")]
        public string? PersonalLife { get; set; }

        [JsonPropertyName("profession")]
        public string? Profession { get; set; }

        [JsonPropertyName("health")]
        public string? Health { get; set; }

        [JsonPropertyName("travel")]
        public string? Travel { get; set; }

        [JsonPropertyName("luck")]
        public string? Luck { get; set; }

        [JsonPropertyName("emotions")]
        public string? Emotions { get; set; }
    }
}
