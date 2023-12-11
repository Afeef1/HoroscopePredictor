namespace HoroscopePredictorAPI.Models
{
    public class HoroscopeData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? SunSign { get; set; }


        public string? PredictionDate { get; set; }


        public PredictionData? Prediction { get; set; }
    }
}
