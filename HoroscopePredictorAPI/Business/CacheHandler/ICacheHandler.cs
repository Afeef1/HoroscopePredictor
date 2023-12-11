using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Business.CacheHandler
{
    public interface ICacheHandler
    {
        HoroscopeData GetCachedData(string zodiacSign, string horoscopeDay);

        int OldEntriesCount();

        void DeleteOlderEntries();


    }
}
