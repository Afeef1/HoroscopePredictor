using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HoroscopePredictorAPI.Business.CacheHandler
{
    public class CacheHandler : ICacheHandler
    {
        private readonly IHoroscopeRepository _horoscopeRepository;

        public CacheHandler(IHoroscopeRepository horoscopeRepository)
        {
            _horoscopeRepository = horoscopeRepository;
        }

        public HoroscopeData GetCachedData(string zodiac, string day)
        {
         
            string date= DateTime.Now.ToShortDateString();
           
            if (day.Equals(Day.tomorrow.ToString()))
            {
                date = DateTime.Now.AddDays(1).ToShortDateString();
               
            }
            else if (day.Equals(Day.yesterday.ToString()))
            {
                date = DateTime.Now.AddDays(-1).ToShortDateString();
            }
            string horoscopeDate = FormatDate(date);
            HoroscopeData cachedHoroscopeData = _horoscopeRepository.GetHoroscopeData(zodiac, horoscopeDate);
            return cachedHoroscopeData;

        }

        private static string FormatDate(string date)
        {
            var dateArray = date.Split('/');
            string formattedDate = dateArray[1] + "-" + dateArray[0] + "-" + dateArray[2];
            return formattedDate;
        }


        public int OldEntriesCount()
        {

            SetDates(out var formattedYesterdayDate, out var formattedTodayDate, out var formattedTomorrowDate);

            int olderPredictionDates = _horoscopeRepository.OldEntriesCount(formattedTodayDate,formattedTomorrowDate,formattedYesterdayDate);
            return olderPredictionDates;

        }


        public void DeleteOlderEntries()
        {

            SetDates(out var formattedYesterdayDate, out var formattedTodayDate, out var formattedTomorrowDate);
            _horoscopeRepository.DeleteOlderEntries(formattedTodayDate,formattedTomorrowDate,formattedYesterdayDate);
        
        }


        private static void SetDates(out string formattedYesterdayDate, out string formattedTodayDate, out string formattedTomorrowDate)
        {
            string yesterdayDate = DateTime.Now.AddDays(-1).ToShortDateString();
            string todayDate = DateTime.Now.ToShortDateString();
            string tomorrowDate = DateTime.Now.AddDays(1).ToShortDateString();
            formattedYesterdayDate = FormatDate(yesterdayDate);
            formattedTodayDate = FormatDate(todayDate);
            formattedTomorrowDate = FormatDate(tomorrowDate);

        }

    }
}
