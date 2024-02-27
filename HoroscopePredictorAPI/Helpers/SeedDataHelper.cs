using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Helpers
{
    public class SeedDataHelper
    {
        private ApiDbContext _apiDbContext;
        public SeedDataHelper(ApiDbContext apiDbContext)
        {
            _apiDbContext = apiDbContext;
        }

        public void SeedData()
        {
            _apiDbContext.Users.Add(new RegisterUser
            {
                Email = "Afeef@gmail.com",
                Id = "1",
                Name = "Afeef",
                Password = HashingHelper.GetHashedPassword("Afeef@123" + "Afeef@gmail.com")
            });
            _apiDbContext.UserCacheData.Add(new UserCacheData
            {
                Zodiac = "aries",
                UserId = "1"
            });
            _apiDbContext.SaveChanges();
        }
    }
}
