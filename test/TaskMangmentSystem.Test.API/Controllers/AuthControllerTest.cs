using System.Net;
using System.Net.Http.Json;
using TaskManagmentSystem.API.DTOs;
using TaskMangmentSystem.Test.API.Fixtures;

namespace TaskMangmentSystem.Test.API.Controllers
{
    [TestClass]

    public class AuthControllerTest : IntegrationTestBase
    {
        #region Postive Part

        [TestMethod]
        public async Task RegisterAsync_When_Return200()
        {
            //Arrange 

            var request = new RegisterRequestDto
            {
                Email = $"usertest{new Random().Next(1000, 9999)}@gmail.com",
                Password = testDataBuilder.Password,
                Role = "User"
            };

            //Act 

            var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task LoginAsync_When_ReturnAccessTokenAnd200()
        {
            //Arrange 

            var userData = await testDataBuilder.CreateAndReturnUser();
            var login = new LoginRequestDto
            {
                Email = userData?.Email,
                Password = testDataBuilder.Password
            };

            //Act 

            var response = await _client.PostAsJsonAsync("/api/Auth/login", login);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }

        #endregion

        #region Postive Part

        [TestMethod]
        public async Task RegisterAsync_WhenInValid_Return400()
        {
            //  //Arrange 

            var request = new RegisterRequestDto
            {
                Email = $"usertest{new Random().Next(1000, 9999)}",  // wrong email format 
                Password = testDataBuilder.Password,
                Role = "User"
            };

            //Act 

            var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task LoginAsync_WhenInValid_Return401()
        {
            //Arrange 

            var rand = new Random();
            var login = new LoginRequestDto
            {
                Email = "rand.Next(10000, 99999).ToString()@gmail.com",
                Password = testDataBuilder.Password
            };

            //Act 

            var response = await _client.PostAsJsonAsync("/api/Auth/login", login);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion

    }
}
