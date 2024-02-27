using HoroscopePredictorAPI.Business.CacheHandler;
using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using HoroscopePredictorAPI.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Business.CacheHandlerTests
{
    [TestClass]
    public class CacheHandlerTests
    {
        private readonly Mock<IHoroscopeRepository> _horoscopeRepository;
        private readonly ICacheHandler _cacheHandler;

        public CacheHandlerTests()
        {
            _horoscopeRepository = new Mock<IHoroscopeRepository>();
            _cacheHandler = new CacheHandler(_horoscopeRepository.Object);
        }

        [TestMethod]
        [DataRow("aries", "today")]
        [DataRow("aries", "tomorrow")]
        [DataRow("aries", "yesterday")]
        public void GetCachedData_ZodiacAndDay_ReturnsCachedHoroscopeData(string zodiac, string day)
        {
            //Arrange
            string date = "19-1-2024";
            HoroscopeData horoscopeData = new HoroscopeData
            {
                
                SunSign = "aries",
                PredictionDate = date,
               
            };
            _horoscopeRepository.Setup(p => p.GetHoroscopeData(It.IsAny<string>(), It.IsAny<string>())).Returns(horoscopeData);

            //Act
            var cachedHoroscopeData = _cacheHandler.GetCachedData(zodiac, day);
            //Assert
            Assert.IsNotNull(cachedHoroscopeData);
            Assert.AreEqual(cachedHoroscopeData.PredictionDate, date);

        }


        [TestMethod]
        [DataRow("aries", "today")]
        [DataRow("aries", "tomorrow")]
        [DataRow("aries", "yesterday")]
        public void GetCachedData_ZodiacAndDay_ReturnsNull(string zodiac, string day)
        {
            //Arrange
            _horoscopeRepository.Setup(p => p.GetHoroscopeData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);

            //Act
            var cachedHoroscopeData = _cacheHandler.GetCachedData(zodiac, day);
            //Assert
            Assert.IsNull(cachedHoroscopeData);
   
        }

        [TestMethod]
        public void OldEntriesCount_ReturnsOlderPredictionDatesCount()
        {
            //Arrange
            int expectedCount = 5;
            _horoscopeRepository.Setup(p => p.OldEntriesCount(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(expectedCount);
           
            //Act
            int oldPredictionDatesCount = _cacheHandler.OldEntriesCount();

            //Assert
            Assert.AreEqual(expectedCount,oldPredictionDatesCount);
        }

        [TestMethod]
        public void DeleteOlderEntries_CallsRepository()
        {
            //Arrange
            _horoscopeRepository.Setup(p => p.DeleteOlderEntries(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //Act
            _cacheHandler.DeleteOlderEntries();

            //Assert
            _horoscopeRepository.Verify(repo => repo.DeleteOlderEntries(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }



    }
}
