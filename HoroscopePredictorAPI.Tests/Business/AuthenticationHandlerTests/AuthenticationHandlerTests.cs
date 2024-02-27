using HoroscopePredictorAPI.Business.AuthenticationHandler;
using HoroscopePredictorAPI.Data_Access.UserRepository;
using HoroscopePredictorAPI.Helpers;
using HoroscopePredictorAPI.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Business.AuthenticationHandlerTests
{
    [TestClass]
    public class AuthenticationHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IConfiguration> _config;
        private readonly IAuthenticationHandler _authenticationHandler;
        public AuthenticationHandlerTests()
        {
            _userRepository=new Mock<IUserRepository>();
            _config = new Mock<IConfiguration>();
            _authenticationHandler = new AuthenticationHandler(_config.Object,_userRepository.Object);

        }

        [TestMethod]
        public async Task RegisterUser_User_RegisterResponseModelConflict()
        {
            //Arrange
             _userRepository.Setup(p => p.DoesUserExist(It.IsAny<string>())).Returns(true);
            

            //Act
            var registerResponseModel = await _authenticationHandler.RegisterUser(new RegisterUser());

            //Assert
            Assert.AreEqual(HttpStatusCode.Conflict, registerResponseModel.StatusCode);
            _userRepository.Verify(p => p.AddUser(new RegisterUser()), Times.Never);
        }

        [TestMethod]
        public async Task RegisterUser_User_RegisterResponseModelCreated()
        {
            //Arrange
            _userRepository.Setup(p => p.DoesUserExist(It.IsAny<string>())).Returns(false);
            _userRepository.Setup(p => p.AddUser(It.IsAny<RegisterUser>()));

            //Act
            var registerResponseModel = await _authenticationHandler.RegisterUser(new RegisterUser());

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, registerResponseModel.StatusCode);
            _userRepository.Verify(p => p.AddUser(It.IsAny<RegisterUser>()), Times.Once);
            
        }

        [TestMethod]
        public void LoginUser_User_ReturnsLoginResponseModelUnauthorized()
        {
            //Arrange
            _userRepository.Setup(p => p.GetCurrentUser(It.IsAny<LoginUser>(), It.IsAny<string>())).Returns(() => null);


            //Act
            var loginResponseModel = _authenticationHandler.LoginUser(new LoginUser());


            //Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized,loginResponseModel.StatusCode);

        }

        [TestMethod]
        public void LoginUser_User_ReturnsLoginResponseModelOk()
        {
            //Arrange
            var registerUser = new RegisterUser
            {
                Name = "Afeef",
                Id = "ghdfd"
            };
            _userRepository.Setup(p => p.GetCurrentUser(It.IsAny<LoginUser>(), It.IsAny<string>())).Returns(registerUser);
            _config.Setup(p => p[Constants.JWT__Key]).Returns("abcdefghihjhftdftjhjhdrsjhyhjgf4hd4hf5AsDAFFASfsfdr");
            _config.Setup(p => p[Constants.JWT__Issuer]).Returns("hgdgfjgj");
            _config.Setup(p => p[Constants.JWT__Audience]).Returns("bjhfydrt");
            

            //Act
            var loginResponseModel = _authenticationHandler.LoginUser(new LoginUser());


            //Assert
            Assert.AreEqual(HttpStatusCode.OK, loginResponseModel.StatusCode);

        }

    }
}
