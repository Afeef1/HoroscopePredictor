using HoroscopePredictorAPI.Data_Access.UserRepository;
using HoroscopePredictorAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Data_Access.UserRepositoryTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;
        private readonly Mock<ApiDbContext> _apiDbContextMock;
        private readonly Mock<DbSet<RegisterUser>> _registerDbSet;
        public UserRepositoryTests()
        {
            _apiDbContextMock = new Mock<ApiDbContext>();
            _registerDbSet = new Mock<DbSet<RegisterUser>>();
            var data = RegisterUserEntries().AsQueryable();

            _registerDbSet.As<IQueryable<RegisterUser>>().Setup(m => m.Provider).Returns(data.Provider);
            _registerDbSet.As<IQueryable<RegisterUser>>().Setup(m => m.Expression).Returns(data.Expression);
            _registerDbSet.As<IQueryable<RegisterUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            _registerDbSet.As<IQueryable<RegisterUser>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            _apiDbContextMock.Setup(p => p.Users).Returns(_registerDbSet.Object);
            _userRepository = new UserRepository(_apiDbContextMock.Object);

        }

        [TestMethod]
        public async Task AddUser_User()
        {
            //Arrange
            var user = new RegisterUser()
            {
                Email = "afeef@gmail.com"
            };
            var registerUserEntries = new List<RegisterUser>();
            _registerDbSet.Setup(p => p.AddAsync(It.IsAny<RegisterUser>(), default))
                .Callback<RegisterUser, CancellationToken>((user, token) => { registerUserEntries.Add(user); })
                .ReturnsAsync(() => null)
                .Verifiable(Times.Once);
            _apiDbContextMock.Setup(p => p.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable(Times.Once);

            //Act
            await _userRepository.AddUser(user);

            //Assert
            Assert.AreEqual(1, registerUserEntries.Count);
            Assert.AreEqual(registerUserEntries[0].Email, user.Email);
            _apiDbContextMock.Verify();
            _registerDbSet.Verify();

        }

        [TestMethod]
        public void GetCurrentUser_UserAndHashedPassword_ReturnsUser()
        {
            //Arrange
            var loginUser = new LoginUser { Email = "afeef@gmail.com" };
            string hashedInputPassword = "abcdefgh";

            //Act
            var user = _userRepository.GetCurrentUser(loginUser,hashedInputPassword);

            //Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(loginUser.Email, user.Email);

        }

        [TestMethod]
        public void GetCurrentUser_UserAndHashedPassword_ReturnsNull()
        {
            //Arrange
            var loginUser = new LoginUser { Email = "afeedasdf@gmail.com" };
            string hashedInputPassword = "abcdefgh";

            //Act
            var user = _userRepository.GetCurrentUser(loginUser, hashedInputPassword);

            //Assert
            Assert.IsNull(user);
           

        }

        [TestMethod]
        public void DoesUserExist_Email_ReturnsTrue()
        {
            //Arrange
            string email = "afeef@gmail.com";

            //Act
            var doesUserExist = _userRepository.DoesUserExist(email);

            //Assert
            Assert.IsTrue(doesUserExist);
        }

        [TestMethod]
        public void DoesUserExist_Email_ReturnsFalse()
        {
            //Arrange
            string email = "afeefsahjsa@gmail.com";

            //Act
            var doesUserExist = _userRepository.DoesUserExist(email);

            //Assert
            Assert.IsFalse(doesUserExist);
        }

        private List<RegisterUser> RegisterUserEntries()
        {
            return new List<RegisterUser>
            {
                 new RegisterUser { Email = "hello@gmail.com", Password = "asbhbhdashj" },
                new RegisterUser { Email = "afeef@gmail.com", Password = "abcdefgh" },
                new RegisterUser { Email = "hii@gmail.com", Password = "ashdsha" }

    };
        }
    }
}
