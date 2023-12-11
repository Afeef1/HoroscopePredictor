using HoroscopePredictorAPI.Models;
using Refit;

namespace HoroscopePredictorAPI.APIHandler
{
    public interface IHoroscopePredictorAPIClient
    {
        [Post("/v1/sun_sign_prediction/daily/{zodiac}")]
        Task<HoroscopeDataExternal> GetHoroscopeDataForTodayAsync(string zodiac, [Authorize("Basic")] string base64AuthKey);

        [Post("/v1/sun_sign_prediction/daily/next/{zodiac}")]
        Task<HoroscopeDataExternal> GetHoroscopeDataForTomorrowAsync(string zodiac, [Authorize("Basic")] string base64AuthKey);

        [Post("/v1/sun_sign_prediction/daily/previous/{zodiac}")]
        Task<HoroscopeDataExternal> GetHoroscopeDataForYesterdayAsync(string zodiac, [Authorize("Basic")] string base64AuthKey);
    }
}
