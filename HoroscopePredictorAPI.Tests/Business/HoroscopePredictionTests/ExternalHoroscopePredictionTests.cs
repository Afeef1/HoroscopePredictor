using Microsoft.Extensions.Configuration;
using HoroscopePredictorAPI.APIHandler;
using HoroscopePredictorAPI.Business.CacheHandler;
using HoroscopePredictorAPI.Business.ExternalHoroscopePrediction;
using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoroscopePredictorAPI.Models;

namespace HoroscopePredictorAPI.Tests.Business.HoroscopePredictionTests
{
    [TestClass]
    public class ExternalHoroscopePredictionTests
    {
        private readonly Mock<IHoroscopeRepository> _horoscopeRepository;
        private readonly Mock<IHoroscopePredictorAPIClient> _horoscopePredictorAPIClient;
        private readonly Mock<ICacheHandler> _cacheHandler;
        private readonly Mock<IConfiguration> _config;
        private readonly IExternalHoroscopePrediction _externalHoroscopePrediction; 
        public ExternalHoroscopePredictionTests()
        {
            _horoscopeRepository = new Mock<IHoroscopeRepository>();
            _horoscopePredictorAPIClient = new Mock<IHoroscopePredictorAPIClient>();
            _cacheHandler = new Mock<ICacheHandler>();
            _config = new Mock<IConfiguration>();
            _externalHoroscopePrediction = new ExternalHoroscopePrediction(_config.Object,_horoscopeRepository.Object,_horoscopePredictorAPIClient.Object,_cacheHandler.Object);
            
        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDay_ReturnsHoroscopeDataFromCache()
        {
            //Arrange
            string zodiac = "aries";
            string day = "today";
            int oldEntriesCount = 0;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(new HoroscopeData()
            {
                Prediction = new PredictionData()
            });
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);

            //Act
            var horoscopeData =  await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(zodiac, base64AuthKey),Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(zodiac, base64AuthKey),Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(zodiac, base64AuthKey),Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Never);


        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDay_ReturnsHoroscopeDataFromCacheAndDeleteOlderEntries()
        {
            //Arrange
            string zodiac = "aries";
            string day = "today";
            int oldEntriesCount = 3;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(new HoroscopeData()
            {
                Prediction = new PredictionData()
            });
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _cacheHandler.Setup(p => p.DeleteOlderEntries());

            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(zodiac, base64AuthKey), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(zodiac, base64AuthKey), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(zodiac, base64AuthKey), Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Once);

        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayToday_ReturnsHoroscopeDataFromCallingExternalAPI()
        {

            //Arrange
            string zodiac = "aries";
            string day = "today";
            int oldEntriesCount = 0;
            string base64AuthKey = "adsdss";  
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal() 
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            })) ;
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Never);

        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayToday_ReturnsHoroscopeDataFromCallingExternalAPIAndDeleteOlderEntries()
        {

            //Arrange
            string zodiac = "aries";
            string day = "today";
            int oldEntriesCount = 3;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal()
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            }));
            _cacheHandler.Setup(p=>p.DeleteOlderEntries());
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Once);

        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayTomorrow_ReturnsHoroscopeDataFromCallingExternalAPI()
        {

            //Arrange
            string zodiac = "aries";
            string day = "tomorrow";
            int oldEntriesCount = 0;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal()
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            }));
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Never);

        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayTomorrow_ReturnsHoroscopeDataFromCallingExternalAPIAndDeleteOlderEntrie()
        {

            //Arrange
            string zodiac = "aries";
            string day = "tomorrow";
            int oldEntriesCount = 3;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal()
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            }));
            _cacheHandler.Setup(p => p.DeleteOlderEntries());
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Once);

        }


        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayYesterday_ReturnsHoroscopeDataFromCallingExternalAPI()
        {

            //Arrange
            string zodiac = "aries";
            string day = "yesterday";
            int oldEntriesCount = 0;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal()
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            }));
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Never);

        }

        [TestMethod]
        public async Task FetchDataFromAPIUsingZodiacSign_ZodiacAndDayYesterday_ReturnsHoroscopeDataFromCallingExternalAPIAndDeleteOlderEntries()
        {

            //Arrange
            string zodiac = "aries";
            string day = "yesterday";
            int oldEntriesCount = 3;
            string base64AuthKey = "adsdss";
            _cacheHandler.Setup(p => p.GetCachedData(It.IsAny<string>(), It.IsAny<string>())).Returns(() => null);
            _cacheHandler.Setup(p => p.OldEntriesCount()).Returns(oldEntriesCount);
            _horoscopePredictorAPIClient.Setup(p => p.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal()
            {
                Prediction = new PredictionDataExternal()
            });
            _horoscopeRepository.Setup(p => p.AddHoroscopeData(new HoroscopeData
            {
                Prediction = new PredictionData()
            }));
            _cacheHandler.Setup(p=>p.DeleteOlderEntries());
            //Act
            var horoscopeData = await _externalHoroscopePrediction.FetchDataFromAPIUsingZodiacSign(zodiac, day);

            //Assert
            Assert.IsNotNull(horoscopeData);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTodayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForTomorrowAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _horoscopePredictorAPIClient.Verify(client => client.GetHoroscopeDataForYesterdayAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _cacheHandler.Verify(handler => handler.DeleteOlderEntries(), Times.Once);

        }
    }
}
