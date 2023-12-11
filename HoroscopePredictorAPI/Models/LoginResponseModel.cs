namespace HoroscopePredictorAPI.Models
{
    public class LoginResponseModel:RegisterResponseModel
    {
        public string JwtToken { get; set; }
    }
}
