using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Data_Access.HoroscopeRepository
{
    public interface IHoroscopeRepository
    {
       

        public Task AddHoroscopeData(HoroscopeData horoscopeData);

        HoroscopeData GetHoroscopeData(string zodiacSign, string horoscopeDay);

        int OldEntriesCount(string todayDate, string tomorrowDate, string yesterdayDate);

        void DeleteOlderEntries(string todayDate, string tomorrowDate, string yesterdayDate);
    }
}
