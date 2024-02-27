using HoroscopePredictorAPI.Business.ExternalHoroscopePrediction;
using HoroscopePredictorAPI.Business.Services;
using HoroscopePredictorAPI.Controllers;
using HoroscopePredictorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Controllers
{
    [TestClass]
    public class HoroscopeControllerTests
    {
        private readonly Mock<IExternalHoroscopePrediction> _externalHoroscopePrediction;
        private readonly Mock<IUserCacheService> _userCacheService;
        private readonly HoroscopeController _horoscopeController;
        private readonly Mock<ClaimsPrincipal> _claimsPrincipal;
        public HoroscopeControllerTests()
        {
            _externalHoroscopePrediction = new Mock<IExternalHoroscopePrediction>();
            _userCacheService = new Mock<IUserCacheService>();
            _horoscopeController = new HoroscopeController(_externalHoroscopePrediction.Object,_userCacheService.Object);
            _claimsPrincipal = new Mock<ClaimsPrincipal>();
            

        }

        [TestMethod]
        public async Task GetHoroscopeByZodiac_ZodiacAndDay_ReturnsOk()
        {
            //Arrange
            string zodiac = "aries";
            string day = "today";
            _externalHoroscopePrediction.Setup(p => p.FetchDataFromAPIUsingZodiacSign(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal());

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "adsdsffe"),
            new Claim(ClaimTypes.Email, "john.doe@example.com"),
        };
            _claimsPrincipal.Setup(p => p.Claims).Returns(claims);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _horoscopeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _userCacheService.Setup(p => p.AddUserHistoryData(It.IsAny<string>(), It.IsAny<string>()));



            //Act
            var horoscopeData = await _horoscopeController.GetHoroscopeByZodiac(zodiac, day);


            //Assert
             Assert.IsNotNull(horoscopeData);
            Assert.IsNotNull((horoscopeData as ObjectResult).Value);

             Assert.IsInstanceOfType(horoscopeData,typeof(OkObjectResult));
             Assert.AreEqual(StatusCodes.Status200OK,(horoscopeData as ObjectResult).StatusCode);
            _userCacheService.Verify(p => p.AddUserHistoryData(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [TestMethod]
        public async Task GetHoroscopeByDateOfBirth_DateOfBirthAndDay_ReturnsOk()
        {
            //Arrange
            string day = "today";
            DateTime dateOfBirth = DateTime.Now;

            _externalHoroscopePrediction.Setup(p => p.FetchDataFromAPIUsingZodiacSign(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HoroscopeDataExternal());

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "adsdsffe"),
            new Claim(ClaimTypes.Email, "john.doe@example.com"),
        };
            _claimsPrincipal.Setup(p => p.Claims).Returns(claims);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _horoscopeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
            _userCacheService.Setup(p => p.AddUserHistoryData(It.IsAny<string>(), It.IsAny<string>()));



            //Act
            var horoscopeData = await _horoscopeController.GetHoroscopeByDateOfBirth(dateOfBirth, day);


            //Assert
            Assert.IsNotNull(horoscopeData);
            Assert.IsNotNull((horoscopeData as ObjectResult).Value);

            Assert.IsInstanceOfType(horoscopeData, typeof(OkObjectResult));
            Assert.AreEqual(StatusCodes.Status200OK, (horoscopeData as ObjectResult).StatusCode);
            _userCacheService.Verify(p => p.AddUserHistoryData(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

    }
}
