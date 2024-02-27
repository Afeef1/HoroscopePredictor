using HoroscopePredictorAPI.Data_Access.HoroscopeRepository;
using HoroscopePredictorAPI.Data_Access.UserCache;
using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Data_Access.HoroscopeRepositoryTests
{
    [TestClass]
    public class HoroscopeRepositoryTests
    {

        private readonly HoroscopeRepository _horoscopeRepository;
        private readonly Mock<ApiDbContext> _apiDbContextMock;
        private readonly Mock<DbSet<HoroscopeData>> _horoscopeDataDbSet;
        private readonly Mock<DbSet<PredictionData>> _predictionDataDbSet;
        public HoroscopeRepositoryTests()
        {
            _apiDbContextMock = new Mock<ApiDbContext>();
            _horoscopeDataDbSet = new Mock<DbSet<HoroscopeData>>();
            _predictionDataDbSet = new Mock<DbSet<PredictionData>>();
            var data = HoroscopeDataEntries().AsQueryable();
            var predictionData = PredictionDataEntries().AsQueryable();

            _horoscopeDataDbSet.As<IQueryable<HoroscopeData>>().Setup(m => m.Provider).Returns(data.Provider);
            _horoscopeDataDbSet.As<IQueryable<HoroscopeData>>().Setup(m => m.Expression).Returns(data.Expression);
            _horoscopeDataDbSet.As<IQueryable<HoroscopeData>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _horoscopeDataDbSet.As<IQueryable<HoroscopeData>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            _apiDbContextMock.Setup(p => p.HoroscopeData).Returns(_horoscopeDataDbSet.Object);

            _predictionDataDbSet.As<IQueryable<PredictionData>>().Setup(m => m.Provider).Returns(predictionData.Provider);
            _predictionDataDbSet.As<IQueryable<PredictionData>>().Setup(m => m.Expression).Returns(predictionData.Expression);
            _predictionDataDbSet.As<IQueryable<PredictionData>>().Setup(m => m.ElementType).Returns(predictionData.ElementType);
            _predictionDataDbSet.As<IQueryable<PredictionData>>().Setup(m => m.GetEnumerator()).Returns(() => predictionData.GetEnumerator());
            _apiDbContextMock.Setup(p => p.PredictionData).Returns(_predictionDataDbSet.Object);

            _horoscopeRepository = new HoroscopeRepository(_apiDbContextMock.Object);
        }

        [TestMethod]
        public async Task AddHoroscopeData_HoroscopeData()
        {
            //Arrange
            var horoscopeData = new HoroscopeData()
            {
                SunSign = "aries",
                PredictionDate = "10-10-2024"
            };
            var horoscopeDataEntries = new List<HoroscopeData>();
            _horoscopeDataDbSet.Setup(p => p.AddAsync(It.IsAny<HoroscopeData>(), default))
                .Callback<HoroscopeData, CancellationToken>((horoscopeData, token) => { horoscopeDataEntries.Add(horoscopeData); })
                .ReturnsAsync(() => null)
                .Verifiable();

            _apiDbContextMock.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable(Times.Once);


            //Act
            await _horoscopeRepository.AddHoroscopeData(horoscopeData);

            //Assert
            Assert.AreEqual(1, horoscopeDataEntries.Count);
            Assert.AreEqual(horoscopeDataEntries[0].SunSign, horoscopeData.SunSign);
            Assert.AreEqual(horoscopeDataEntries[0].PredictionDate, horoscopeData.PredictionDate);
            _apiDbContextMock.Verify();
            _horoscopeDataDbSet.Verify();
        }

        [TestMethod]
        public void OldEntriesCount_TodayDateTomorrowDateAndYesterdayDate_CountOldEntries()
        {
            //Arrange
            string todayDate = "11-10-2024";
            string tomorrowDate = "12-10-2024";
            string yesterdayDate = "10-10-2024";
            int expectedCount = 2;
            //Act
            var oldEntriesCount = _horoscopeRepository.OldEntriesCount(todayDate, tomorrowDate, yesterdayDate);

            //Assert
            Assert.AreEqual(expectedCount, oldEntriesCount);


        }

        [TestMethod]
        public void GetHoroscopeData_ZodiacAndHoroscopeDate_HoroscopeData()
        {
            //Arrange
            string zodiac = "aries";
            string horoscopeDate = "12-10-2024";
            string predictionPersonalLife = "Your Personal Life";

            //Act
            var horoscopeData = _horoscopeRepository.GetHoroscopeData(zodiac, horoscopeDate);

            //Assert
            Assert.AreEqual(zodiac, horoscopeData.SunSign);
            Assert.AreEqual(horoscopeDate, horoscopeData.PredictionDate);
            Assert.AreEqual(predictionPersonalLife, horoscopeData.Prediction.PersonalLife);

        }

        [TestMethod]
        public void DeleteOlderEntries_TodayDateTomorrowDateAndYesterdayDate()
        {
            //Arrange
            string todayDate = "11-10-2024";
            string tomorrowDate = "12-10-2024";
            string yesterdayDate = "10-10-2024";
            int expectedCount = 3;
            var horoscopeDataEntries = HoroscopeDataEntries();
            var predictionDataEntries = PredictionDataEntries();

            _horoscopeDataDbSet.Setup(p => p.RemoveRange(It.IsAny<List<HoroscopeData>>()))
                .Callback<IEnumerable<HoroscopeData>>((horoscopeData) =>
                {
                    horoscopeDataEntries.RemoveAt(0);
                    horoscopeDataEntries.RemoveAt(2);
                }
                )
                .Verifiable(Times.Once);

            _predictionDataDbSet.Setup(p => p.RemoveRange(It.IsAny<List<PredictionData>>()))
               .Callback<IEnumerable<PredictionData>>((predictionData) =>
               {
                 predictionDataEntries.RemoveAt(0);
                   predictionDataEntries.RemoveAt(2);
               }
               )
               .Verifiable(Times.Once);
            _apiDbContextMock.Setup(p => p.SaveChanges()).Returns(2).Verifiable(Times.Once);
            //Act
            _horoscopeRepository.DeleteOlderEntries(todayDate, tomorrowDate, yesterdayDate);

            //Assert
            Assert.AreEqual(expectedCount,horoscopeDataEntries.Count);
            Assert.AreEqual(expectedCount,predictionDataEntries.Count);
            _apiDbContextMock.Verify();
            _horoscopeDataDbSet.Verify();
            _predictionDataDbSet.Verify();


        }


        private List<HoroscopeData> HoroscopeDataEntries()
        {
            return new List<HoroscopeData>
            {
              new HoroscopeData(){ Id="abcd",SunSign="libra",PredictionDate="20-09-2024",Prediction=new PredictionData(){Id="abc123",PersonalLife="My Personal Life", Profession="My profession", Health="My health", Emotions="My Emotions", Luck="My luck", Travel="My travel" } },
              new HoroscopeData(){ Id="efgh",SunSign="aries",PredictionDate="12-10-2024",Prediction=new PredictionData(){Id="efg345",PersonalLife="Your Personal Life", Profession="Your profession", Health="Your health", Emotions="Your Emotions", Luck="Your luck", Travel="Your travel"} },
              new HoroscopeData(){ Id="ijkl",SunSign="libra",PredictionDate="25-08-2024",Prediction=new PredictionData() {Id="abc456",PersonalLife="Hey Personal Life", Profession="Hey profession", Health="Hey health", Emotions="Hey Emotions", Luck="Hey luck", Travel="Hey travel" }},
              new HoroscopeData(){ Id="mnop",SunSign="libra",PredictionDate="10-10-2024",Prediction=new PredictionData() {Id="zxc123", PersonalLife="No Personal Life", Profession="No profession", Health="No health", Emotions="No Emotions", Luck="No luck", Travel="No travel" }},
              new HoroscopeData(){ Id="qrst",SunSign="libra",PredictionDate="11-10-2024",Prediction=new PredictionData()  {Id="qwe456", PersonalLife="Balanced Personal Life", Profession="Balanced profession", Health="Balanced health", Emotions="Balanced Emotions", Luck="Balanced luck", Travel="Balanced travel"}},


    };
        }

        private List<PredictionData> PredictionDataEntries()
        {
            return new List<PredictionData>
            {
                new PredictionData {Id="abc123",PersonalLife="My Personal Life", Profession="My profession", Health="My health", Emotions="My Emotions", Luck="My luck", Travel="My travel" },
                new PredictionData {Id="efg345",PersonalLife="Your Personal Life", Profession="Your profession", Health="Your health", Emotions="Your Emotions", Luck="Your luck", Travel="Your travel" },
                new PredictionData {Id="abc456",PersonalLife="Hey Personal Life", Profession="Hey profession", Health="Hey health", Emotions="Hey Emotions", Luck="Hey luck", Travel="Hey travel" },
                new PredictionData {Id="zxc123", PersonalLife="No Personal Life", Profession="No profession", Health="No health", Emotions="No Emotions", Luck="No luck", Travel="No travel" },
                new PredictionData {Id="qwe456", PersonalLife="Balanced Personal Life", Profession="Balanced profession", Health="Balanced health", Emotions="Balanced Emotions", Luck="Balanced luck", Travel="Balanced travel" }
            };
        }
    }
}
