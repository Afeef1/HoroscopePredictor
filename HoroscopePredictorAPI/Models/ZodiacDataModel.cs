using System.Text.Json.Serialization;

namespace HoroscopePredictorAPI.Models
{
    public class ZodiacDataModel
    {
            [JsonPropertyName("zodiac")]
            public string? Zodiac { get; set; }

            [JsonPropertyName("startDate")]
            public string? StartDate { get; set; }

            [JsonPropertyName("endDate")]
            public string? EndDate { get; set; }
    
    }
}
