using HoroscopePredictorAPI.Business.CacheHandler;
using HoroscopePredictorAPI.Business.Services;
using HoroscopePredictorAPI.Data_Access.UserCache;
using HoroscopePredictorAPI.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Business.Services
{
    [TestClass]
    public class UserCacheServiceTests
    {
        private readonly Mock<IUserCacheRepository> _userCacheRepository;
        private readonly IUserCacheService _userCacheService;
        public UserCacheServiceTests()
        {
            _userCacheRepository = new Mock<IUserCacheRepository>();
            _userCacheService = new UserCacheService(_userCacheRepository.Object);

        }

        [TestMethod]
        public void GetUserHistoryData_ExistingUserId_ReturnsZodiacSearchHistory()
        {
            //Arrange
            string userId = "abcdefghi";
            var expectedResult = new List<string> { "aries", "capricorn" };
            _userCacheRepository.Setup(p => p.GetUserZodaicHistory(userId)).Returns(expectedResult);

            //Act
            var zodiacSearchHistory = _userCacheService.GetUserHistoryData(userId);

            //Assert
            Assert.IsNotNull(zodiacSearchHistory);
            CollectionAssert.AreEqual(expectedResult, zodiacSearchHistory);

        }

        [TestMethod]
        public void GetUserHistoryData_NonExistingUserId_ReturnsNullZodiacSearchHistory()
        {
            //Arrange
            

            _userCacheRepository.Setup(p => p.GetUserZodaicHistory(It.IsAny<string>())).Returns(() => null);

            //Act
            var zodiacSearchHistory = _userCacheService.GetUserHistoryData(It.IsAny<string>());

            //Assert
            Assert.IsNull(zodiacSearchHistory);

        }

        [TestMethod]
        public void AddUserHistoryData_ExistingZodiacAndUserId_UpdatesExistingCache()
        {
            //Arrange
            string zodiac = "aries";
            string userId = "abcdef";
            var userCacheData = new UserCacheData()
            {
                Id = "zxcvbnm",
                Zodiac = zodiac,
                UserId = userId,
                TimeStamp = DateTime.Now
            };
            _userCacheRepository.Setup(p => p.ZodiacData(It.IsAny<string>(), It.IsAny<string>())).Returns(userCacheData);


            //Act
            _userCacheService.AddUserHistoryData(zodiac,userId);

            //Assert
            _userCacheRepository.Verify(repo=>repo.UpdateUserCacheData(userCacheData),Times.Once);
            _userCacheRepository.Verify(repo => repo.AddUserCacheData(It.IsAny<UserCacheData>()), Times.Never);
            _userCacheRepository.Verify(repo => repo.RemoveUserCacheData(It.IsAny<string>()), Times.Never);

        }

        [TestMethod]
        public void AddUserHistoryData_NonExistingZodiacAndUserId_AddsNewCacheData()
        {
            //Arrange
            string zodiac = "aries";
            string userId = "abcdef";
            int userEntriesCount = 4;

            _userCacheRepository.Setup(p => p.ZodiacData(It.IsAny<string>(), It.IsAny<string>())).Returns(()=>null);
            _userCacheRepository.Setup(p => p.AddUserCacheData(It.IsAny<UserCacheData>()));
            _userCacheRepository.Setup(p => p.UserEntriesCount(userId)).Returns(userEntriesCount);

            //Act
            _userCacheService.AddUserHistoryData(zodiac, userId);

            //Assert
            _userCacheRepository.Verify(repo => repo.UpdateUserCacheData(It.IsAny<UserCacheData>()), Times.Never);
            _userCacheRepository.Verify(repo => repo.AddUserCacheData(It.IsAny<UserCacheData>()), Times.Once);
            _userCacheRepository.Verify(repo => repo.RemoveUserCacheData(It.IsAny<string>()), Times.Never);

        }

        [TestMethod]
        public void AddUserHistoryData_NonExistingZodiacAndUserId_AddsNewCacheDataAndRemovesExtraCacheData()
        {
            //Arrange
            string zodiac = "aries";
            string userId = "abcdef";
            int userEntriesCount = 6;

            _userCacheRepository.Setup(p => p.ZodiacData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _userCacheRepository.Setup(p => p.AddUserCacheData(It.IsAny<UserCacheData>()));
            _userCacheRepository.Setup(p => p.UserEntriesCount(userId)).Returns(userEntriesCount);
            _userCacheRepository.Setup(p => p.RemoveUserCacheData(userId));

            //Act
            _userCacheService.AddUserHistoryData(zodiac, userId);

            //Assert
            _userCacheRepository.Verify(repo => repo.UpdateUserCacheData(It.IsAny<UserCacheData>()), Times.Never);
            _userCacheRepository.Verify(repo => repo.AddUserCacheData(It.IsAny<UserCacheData>()), Times.Once);
            _userCacheRepository.Verify(repo => repo.RemoveUserCacheData(It.IsAny<string>()), Times.Once);

        }
    }
}
