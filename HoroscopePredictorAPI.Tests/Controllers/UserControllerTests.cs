using Azure;
using HoroscopePredictorAPI.Business.AuthenticationHandler;
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
    public class UserControllerTests
    {
        private readonly Mock<IAuthenticationHandler> _authenticationHandler;
        private readonly Mock<IUserCacheService> _userCacheService;
        private readonly UserController _userController;
        private readonly Mock<ClaimsPrincipal> _claimsPrincipal;
        public UserControllerTests()
        {
            _authenticationHandler = new Mock<IAuthenticationHandler>();
            _userCacheService = new Mock<IUserCacheService>();
            _userController = new UserController(_authenticationHandler.Object,_userCacheService.Object);
            _claimsPrincipal = new Mock<ClaimsPrincipal>();
        }

        [TestMethod]
        public async Task Register_User_ReturnsConfict()
        {
            //Arrange
            var registerResponseModel = new RegisterResponseModel()
            {
                StatusCode = HttpStatusCode.Conflict
            }; 
                
      
            _authenticationHandler.Setup(p=>p.RegisterUser(It.IsAny<RegisterUser>())).ReturnsAsync(registerResponseModel);

            //Act
            var response = await _userController.Register(new RegisterUser());

            //Assert
            Assert.AreEqual(StatusCodes.Status409Conflict,(response as ObjectResult).StatusCode);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ConflictObjectResult));

        }


        [TestMethod]
        public async Task Register_User_ReturnsCreated()
        {
            //Arrange
            var registerResponseModel = new RegisterResponseModel()
            {
                StatusCode = HttpStatusCode.Created
            };


            _authenticationHandler.Setup(p => p.RegisterUser(It.IsAny<RegisterUser>())).ReturnsAsync(registerResponseModel);

            //Act
            var response = await _userController.Register(new RegisterUser());

            //Assert
            Assert.AreEqual(StatusCodes.Status201Created, (response as ObjectResult).StatusCode);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
            //Check this
        }

        [TestMethod]
        public void Login_User_ReturnsUnauthorized()
        {
            //Arrange
            var loginResponseModel = new LoginResponseModel()
            {
                StatusCode= HttpStatusCode.Unauthorized
            };

           _authenticationHandler.Setup(p=>p.LoginUser(It.IsAny<LoginUser>())).Returns(loginResponseModel);

            //Act
            var response = _userController.Login(new LoginUser());

            //Assert
            Assert.AreEqual(StatusCodes.Status401Unauthorized, (response as ObjectResult).StatusCode);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public void Login_User_ReturnsOK()
        {
            //Arrange
            var loginResponseModel = new LoginResponseModel()
            {
                StatusCode = HttpStatusCode.OK
            };

            _authenticationHandler.Setup(p => p.LoginUser(It.IsAny<LoginUser>())).Returns(loginResponseModel);

            //Act
            var response = _userController.Login(new LoginUser());

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, (response as ObjectResult).StatusCode);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public void UserSearchHistory_ReturnsOK()
        {
            //Arrange
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "adsdsffe"),
            new Claim(ClaimTypes.Email, "john.doe@example.com"),
        };
            _claimsPrincipal.Setup(p => p.Claims).Returns(claims);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _userController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _userCacheService.Setup(p => p.GetUserHistoryData(It.IsAny<string>())).Returns(new List<string>());

            //Act
            var response = _userController.UserSearchHistory();

            //Assert
            Assert.IsNotNull (response);
            Assert.AreEqual(StatusCodes.Status200OK, (response as ObjectResult).StatusCode);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));

        }


    }
}
