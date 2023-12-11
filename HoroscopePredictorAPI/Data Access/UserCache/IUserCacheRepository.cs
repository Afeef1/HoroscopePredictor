using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Data_Access.UserCache
{
    public interface IUserCacheRepository
    {
        public void AddUserCacheData(UserCacheData userCacheData);

        public void RemoveUserCacheData(string userId);

        public UserCacheData? ZodiacData(string zodiac, string userId);

        public List<string> GetUserZodaicHistory(string userId);
        public void UpdateUserCacheData(UserCacheData userCacheData);

        public int UserEntriesCount(string userId);
    }
}
