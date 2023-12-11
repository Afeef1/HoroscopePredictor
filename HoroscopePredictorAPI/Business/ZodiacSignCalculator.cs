using HoroscopePredictorAPI.Models;
using System.Text.Json;

namespace HoroscopePredictorAPI.Business
{
    public class ZodiacSignCalculator
    {
        public static string GetZodiacSignFromDateOfBirth(DateTime userDateOfBirth)
        {
            string dateOfBirth = userDateOfBirth.ToString("dd-MM");
            string currentDirectory = Directory.GetCurrentDirectory();
            string zodiacSignFilePath = Path.Combine(currentDirectory, @"wwwroot/ZodiacSign.json");

            List<ZodiacDataModel> zodiacSigns = LoadZodiacSigns(zodiacSignFilePath);
            string zodiacSign = FindZodiacSign(dateOfBirth, zodiacSigns);
            return zodiacSign;

        }

        public static List<ZodiacDataModel> LoadZodiacSigns(string jsonFilePath)
        {
            string jsonString = File.ReadAllText(jsonFilePath);
            List<ZodiacDataModel> zodiacSigns = JsonSerializer.Deserialize<List<ZodiacDataModel>>(jsonString);
            return zodiacSigns;
        }
        public static string FindZodiacSign(string dateOfBirth, List<ZodiacDataModel> zodiacSigns)
        {
            DateTime dob = DateTime.ParseExact(dateOfBirth, "dd-MM", null);

            var matchingSign = zodiacSigns.FirstOrDefault(sign =>
                dob >= DateTime.ParseExact(sign.StartDate, "dd-MM", null) &&
                dob <= DateTime.ParseExact(sign.EndDate, "dd-MM", null) ||
                (sign.Zodiac == "Capricorn" && (dob >= DateTime.ParseExact(sign.StartDate, "dd-MM", null) ||
                dob <= DateTime.ParseExact(sign.EndDate, "dd-MM", null))));
            return matchingSign != null ? matchingSign.Zodiac : "Unknown Zodiac";
        }
    }
}
