using HoroscopePredictorAPI.Data_Access;
using HoroscopePredictorAPI.Data_Access.UserCache;
using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Business.Services
{
    public class UserCacheService : IUserCacheService
    {
        private readonly IUserCacheRepository _userCacheRepository;

        public UserCacheService(IUserCacheRepository userCacheRepository)
        {
            _userCacheRepository = userCacheRepository;
        }
        public void AddUserHistoryData(string zodiac, string userId)
        {
            var existingZodiacData = _userCacheRepository.ZodiacData(zodiac, userId);
            if (existingZodiacData != null)
            {
                _userCacheRepository.UpdateUserCacheData(existingZodiacData);
            }

            else
            {
                var userCacheData = new UserCacheData();
                userCacheData.UserId = userId;
                userCacheData.Zodiac = zodiac;
                userCacheData.TimeStamp = DateTime.Now;
                _userCacheRepository.AddUserCacheData(userCacheData);
                int userEntriesCount = _userCacheRepository.UserEntriesCount(userId);
                if (userEntriesCount > 5)
                {
                    _userCacheRepository.RemoveUserCacheData(userId);
                }
            }
        }

        public List<string> GetUserHistoryData(string userId)
        {
            var zodiacSearchHistory = _userCacheRepository.GetUserZodaicHistory(userId);
            return zodiacSearchHistory;
        }
    }
}
