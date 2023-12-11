using HoroscopePredictorAPI.APIHandler;
using HoroscopePredictorAPI.Data_Access;
using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HoroscopePredictorAPI.Data_Access.HoroscopeRepository
{
    public class HoroscopeRepository : IHoroscopeRepository
    {
        private readonly ApiDbContext _apiDbContext;

        public HoroscopeRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext=apiDbContext;
        }
        public async Task AddHoroscopeData(HoroscopeData horoscopeData)
        {
           
           await _apiDbContext.HoroscopeData.AddAsync(horoscopeData);
           await _apiDbContext.SaveChangesAsync();
        }

        public HoroscopeData GetHoroscopeData(string zodiac, string horoscopeDate)
        {
            return _apiDbContext.HoroscopeData.Include(horoscope => horoscope.Prediction).AsQueryable()
                .FirstOrDefault(horoscope => EF.Functions.Collate(horoscope.SunSign, "Latin1_General_CI_AI") == zodiac && horoscope.PredictionDate.Equals(horoscopeDate));
        }


        public int OldEntriesCount(string todayDate,string tomorrowDate, string yesterdayDate)
        {
            return _apiDbContext.HoroscopeData.Where(date => date.PredictionDate != todayDate && date.PredictionDate != yesterdayDate && date.PredictionDate != tomorrowDate).ToList().Count();
        }

        public void DeleteOlderEntries(string todayDate, string tomorrowDate, string yesterdayDate)
        {

            var zodiacData = _apiDbContext.HoroscopeData.Where(date => date.PredictionDate != todayDate && date.PredictionDate != yesterdayDate && date.PredictionDate != tomorrowDate).Include(c => c.Prediction).ToList();

            var predictionDataId = zodiacData.Select(horoscope => horoscope.Prediction.Id);
            var entriesToDelete = _apiDbContext.PredictionData
            .Where(entry => predictionDataId.Contains(entry.Id))
            .ToList();

            _apiDbContext.HoroscopeData.RemoveRange(zodiacData);
            _apiDbContext.PredictionData.RemoveRange(entriesToDelete);
            _apiDbContext.SaveChanges();

        }




    }
}
