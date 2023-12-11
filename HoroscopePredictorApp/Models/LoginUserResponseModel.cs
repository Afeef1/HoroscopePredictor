namespace HoroscopePredictorApp.Models
{
    public class LoginUserResponseModel:RegisterUserResponseModel
    {
        public string JwtToken { get; set; }
    }
}
