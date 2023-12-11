using HoroscopePredictorAPI.APIHandler;
using HoroscopePredictorAPI.Business.CacheHandler;
using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using System.Text;

namespace HoroscopePredictorAPI.Business.ExternalHoroscopePrediction

{
    public class ExternalHoroscopePrediction : IExternalHoroscopePrediction
    {
        private readonly IConfiguration _config;
        private readonly IHoroscopeRepository _horoscopeRepository;
        private readonly IHoroscopePredictorAPIClient _horoscopePredictorAPIClient;
        private readonly ICacheHandler _cacheHandler;
        private string _base64AuthKey;

        public ExternalHoroscopePrediction(IConfiguration configuration, IHoroscopeRepository horoscopeRepository, IHoroscopePredictorAPIClient horoscopePredictorAPIClient, ICacheHandler cacheHandler)
        {


            _horoscopeRepository = horoscopeRepository;
            _horoscopePredictorAPIClient=horoscopePredictorAPIClient;
            _cacheHandler = cacheHandler;
            _config = configuration;
            SetAuthKey();
        }
        
        private void SetAuthKey()
        {

            string userId = _config[Constants.ExternalAPIConfigurations__UserId];
            string apiKey = _config[Constants.ExternalAPIConfigurations__APIKey];
            string authKey = userId + ":" + apiKey;
            byte[] authKeyBytes = Encoding.UTF8.GetBytes(authKey);
            _base64AuthKey = Convert.ToBase64String(authKeyBytes);
        }

        public async Task<HoroscopeDataExternal> FetchDataFromAPIUsingZodiacSign(string zodiac, string day)
        {

            HoroscopeDataExternal horoscopeData;
            var cachedHoroscopeData = _cacheHandler.GetCachedData(zodiac, day);

            if (cachedHoroscopeData != null)
            {
                horoscopeData = ModelMapper.MappedHoroscopeDataFromInternal(cachedHoroscopeData);
            }
            else
            {
                if (day.Equals(Day.today.ToString()))
                {
                    horoscopeData = await _horoscopePredictorAPIClient.GetHoroscopeDataForTodayAsync(zodiac, _base64AuthKey);


                }
                else if (day.Equals(Day.tomorrow.ToString()))
                {
                    horoscopeData = await _horoscopePredictorAPIClient.GetHoroscopeDataForTomorrowAsync(zodiac, _base64AuthKey);
                }
                else
                {
                    horoscopeData = await _horoscopePredictorAPIClient.GetHoroscopeDataForYesterdayAsync(zodiac, _base64AuthKey);
                }
                var horoscopeDataInternal = ModelMapper.MappedHoroscopeDataFromExternal(horoscopeData);
                await _horoscopeRepository.AddHoroscopeData(horoscopeDataInternal);
            }

            int oldEntriesCount = _cacheHandler.OldEntriesCount();
            if (oldEntriesCount > 0)
            {
                _cacheHandler.DeleteOlderEntries();
            }
            return horoscopeData;
        }


    }
}
