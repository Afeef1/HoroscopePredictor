namespace HoroscopePredictorAPI.Business.Services
{
    public interface IUserCacheService
    {
        public void AddUserHistoryData(string zodiac, string userId);

        public List<string> GetUserHistoryData(string userId);
    }
}
