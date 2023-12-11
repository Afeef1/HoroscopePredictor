using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Data_Access.UserCache
{
    public class UserCacheRepository : IUserCacheRepository
    {
        private ApiDbContext _apiDbContext;
        public UserCacheRepository(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }
        public void AddUserCacheData(UserCacheData userCacheData)
        {
            _apiDbContext.UserCacheData.Add(userCacheData);
            _apiDbContext.SaveChanges();
        }

        public UserCacheData? ZodiacData(string zodiac, string userId)
        {
            var zodiacData = _apiDbContext.UserCacheData.FirstOrDefault(user => user.UserId.Equals(userId) && user.Zodiac.Equals(zodiac));
            return zodiacData;
        }

        public List<string> GetUserZodaicHistory(string userId)
        {
            var searchedZodiac = _apiDbContext.UserCacheData.Where(user => user.UserId.Equals(userId))
                                 .OrderByDescending(user => user.TimeStamp).Select(user => user.Zodiac)
                                 .ToList();
            return searchedZodiac;
        }

        public void RemoveUserCacheData(string userId)
        {
            var zodiacData = _apiDbContext.UserCacheData.Where(user => user.UserId.Equals(userId))
                              .OrderBy(user => user.TimeStamp)
                              .ToList();

            var earliestEntry = zodiacData.First();
            _apiDbContext.UserCacheData.Remove(earliestEntry);
            _apiDbContext.SaveChanges();

        }

        public void UpdateUserCacheData(UserCacheData userCacheData)
        {
            userCacheData.TimeStamp = DateTime.Now;
            _apiDbContext.UserCacheData.Update(userCacheData);
            _apiDbContext.SaveChanges();

        }

        public int UserEntriesCount(string userId)
        {
            return _apiDbContext.UserCacheData.Where(user => user.UserId.Equals(userId))
                    .ToList().Count;
        }

    }
}
