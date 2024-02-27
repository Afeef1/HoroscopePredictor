using HoroscopePredictorAPI.Data_Access.UserCache;
using HoroscopePredictorAPI.Data_Access.UserRepository;
using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Data_Access.UserCacheRepositoryTests
{
    [TestClass]
    public class UserCacheRepositoryTests
    {
        private readonly UserCacheRepository _userCacheRepository;
        private readonly Mock<ApiDbContext> _apiDbContextMock;
        private readonly Mock<DbSet<UserCacheData>> _userCacheDataDbSet;
        
        public UserCacheRepositoryTests()
        {
            _apiDbContextMock = new Mock<ApiDbContext>();
            _userCacheDataDbSet = new Mock<DbSet<UserCacheData>>();
            var data = UserCacheDataEntries().AsQueryable();

            _userCacheDataDbSet.As<IQueryable<UserCacheData>>().Setup(m => m.Provider).Returns(data.Provider);
            _userCacheDataDbSet.As<IQueryable<UserCacheData>>().Setup(m => m.Expression).Returns(data.Expression);
            _userCacheDataDbSet.As<IQueryable<UserCacheData>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _userCacheDataDbSet.As<IQueryable<UserCacheData>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            _apiDbContextMock.Setup(p => p.UserCacheData).Returns(_userCacheDataDbSet.Object);
            _userCacheRepository = new UserCacheRepository(_apiDbContextMock.Object);
        }


        [TestMethod]
        public void AddUser_UserCacheData()
        {
            //Arrange
            var userCacheData = new UserCacheData()
            {
                Zodiac = "aries"
            };
            var userCacheDataEntries = new List<UserCacheData>();
            _userCacheDataDbSet.Setup(p => p.Add(It.IsAny<UserCacheData>()))
                .Callback<UserCacheData>((userCacheData) => { userCacheDataEntries.Add(userCacheData); })
                .Returns(() => null)
                .Verifiable(Times.Once);
            _apiDbContextMock.Setup(p => p.SaveChanges()).Returns(1).Verifiable(Times.Once);

            //Act
            _userCacheRepository.AddUserCacheData(userCacheData);

            //Assert
            Assert.AreEqual(1, userCacheDataEntries.Count);
            Assert.AreEqual(userCacheDataEntries[0].Zodiac, userCacheData.Zodiac);
            _apiDbContextMock.Verify();
            _userCacheDataDbSet.Verify();

        }

        [TestMethod]
        public void UpdateUserCacheData_UserCacheData()
        {
            //Arrange
            var oldUserCacheData = new UserCacheData()
            {
                TimeStamp = DateTime.MinValue
            };

            var newUserCacheData = new UserCacheData()
            {
                TimeStamp = DateTime.Now
            };

            var userCacheDataEntries = new List<UserCacheData>()
            {
                oldUserCacheData
            };

            _userCacheDataDbSet.Setup(p => p.Update(It.IsAny<UserCacheData>()))
                .Callback<UserCacheData>((newUserCacheData) => { userCacheDataEntries[0] = newUserCacheData; })
                .Returns(() => null)
                .Verifiable();
            _apiDbContextMock.Setup(p => p.SaveChanges()).Returns(1).Verifiable(Times.Once);

            //Act
            _userCacheRepository.UpdateUserCacheData(newUserCacheData);

            //Assert
            Assert.AreEqual(1, userCacheDataEntries.Count);
            Assert.AreEqual(userCacheDataEntries[0].TimeStamp, newUserCacheData.TimeStamp);
            _apiDbContextMock.Verify();
            _userCacheDataDbSet.Verify();


        }

        [TestMethod]
        public void RemoveUserCache_UserId()
        {
            //Arrange
            string userId = "abcd";
            int expectedCount = 2;
            var userCacheDataEntries = UserCacheDataEntries().ToList();
            _userCacheDataDbSet.Setup(p => p.Remove(It.IsAny<UserCacheData>()))
                .Callback<UserCacheData>((data) => { userCacheDataEntries.RemoveAt(0); })
                .Returns(() => null)
                .Verifiable(Times.Once);
            _apiDbContextMock.Setup(p => p.SaveChanges()).Returns(1).Verifiable(Times.Once);

            //Act
            _userCacheRepository.RemoveUserCacheData(userId);

            //Assert
            Assert.AreEqual(expectedCount, userCacheDataEntries.Count);
            _apiDbContextMock.Verify();
            _userCacheDataDbSet.Verify();

        }

        [TestMethod]
        public void UserEntriesCount_UserId_EntriesCount()
        {
            //Arrange
            string userId = "abcd";
            int expectedCount = 2;


            //Act
            var userEntriesCount = _userCacheRepository.UserEntriesCount(userId);

            //Assert
            Assert.AreEqual(expectedCount,userEntriesCount);
        }

        [TestMethod]
        public void GetUserZodiacHistory_UserId_ReturnsListOfSearchedZodiac()
        {
            //Arrange
            string userId = "abcd";
            int expectedCount = 2;
            string expectedFirstZodiac = "capricorn";
            string expectedSecondZodiac = "libra";
            //Act
            var zodiacData = _userCacheRepository.GetUserZodaicHistory(userId);

            //Assert
            Assert.AreEqual(expectedCount, zodiacData.Count);
            Assert.AreEqual(expectedFirstZodiac, zodiacData[0]);
            Assert.AreEqual(expectedSecondZodiac, zodiacData[1]);
        }


        [TestMethod]
        public void ZodiacData_ZodiacAndUserId_ReturnsZodiacData()
        {
            //Arrange
            string userId = "abcd";
            string zodiac = "libra";
            DateTime timeStamp = DateTime.MinValue;
            //Act
            var zodiacData = _userCacheRepository.ZodiacData(zodiac, userId);

            //Assert
            Assert.AreEqual(zodiac,zodiacData.Zodiac);
            Assert.AreEqual(userId,zodiacData.UserId);
            Assert.AreEqual(timeStamp,zodiacData.TimeStamp);


        }

        private List<UserCacheData> UserCacheDataEntries()
        {
            return new List<UserCacheData>
            {
                new UserCacheData() { Zodiac="capricorn",UserId="abcd",TimeStamp = DateTime.MinValue.AddDays(5)},
                new UserCacheData() { Zodiac="libra",UserId="abcd",TimeStamp = DateTime.MinValue},
                new UserCacheData() {Zodiac="leo", UserId="cdef",TimeStamp=DateTime.MinValue.AddDays(3)}
    };
        }

    }
}
