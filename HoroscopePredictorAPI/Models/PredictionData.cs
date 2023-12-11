namespace HoroscopePredictorAPI.Models
{
    public class PredictionData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? PersonalLife { get; set; }

        public string? Profession { get; set; }

        public string? Health { get; set; }

        public string? Travel { get; set; }

        public string? Luck { get; set; }

        public string? Emotions { get; set; }
    }
}
