using HoroscopePredictorAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoroscopePredictorAPI.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTests
    {
        private readonly ErrorController _errorController;
        public ErrorControllerTests()
        {
          _errorController = new ErrorController();
        }

        [TestMethod]
        public void HandleError_ReturnsInternalServerError()
        {
            //Arrange


            //Act
            var response = _errorController.HandleError();

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(StatusCodes.Status500InternalServerError, (response as ObjectResult).StatusCode);
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
        }
    }
}
