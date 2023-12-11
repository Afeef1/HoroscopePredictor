using HoroscopePredictorApp.Models;
using HoroscopePredictorApp.ViewModels;
using Refit;

namespace HoroscopePredictorApp.Data_Access
{
    public interface IHoroscopePredictorAPIClient
    {
        [Get("/api/horoscope/{zodiac}?day={horoscopeDay}")]
        Task<HoroscopeData> GetHoroscopeDataFromZodiacAsync(string zodiac, string horoscopeDay);

        [Get("/api/horoscope/dob?dateOfBirth={dateOfBirth}&day={horoscopeDay}")]
        Task<HoroscopeData> GetHoroscopeDataFromDateOfBirthAsync(DateTime dateOfBirth, string horoscopeDay);

        [Post("/api/user/register")]
        Task<RegisterUserResponseModel> Register([Body] RegisterUser user);
        [Post("/api/user/login")]
        Task<LoginUserResponseModel> Login([Body] LoginUser user);

        [Get("/api/user/history")]
        Task<List<string>> History();
        

        
    }
}
