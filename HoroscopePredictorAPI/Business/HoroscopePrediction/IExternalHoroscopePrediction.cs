using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Business.ExternalHoroscopePrediction
{
    public interface IExternalHoroscopePrediction
    {
        Task<HoroscopeDataExternal> FetchDataFromAPIUsingZodiacSign(string zodiacSign, string horoscopeDay);

    }
}
