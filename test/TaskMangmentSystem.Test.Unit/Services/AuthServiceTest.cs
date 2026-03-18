using TaskManagmentSystem.API.Entities;
using TaskMangmentSystem.Test.Unit.Common;

namespace TaskMangmentSystem.Test.Unit.Services
{
    [TestClass]
    public class AuthServiceTest : AuthServiceTestBase
    {
        #region Postive Part 

        [TestMethod]
        public async Task RegisterAsync_WhenValid_ReturnTrue()
        {
            //Arrange 
            var user = ReturnUser();

            //Act 
            var result = await _authService.RegisterAsync(user);

            //Assert

            Assert.IsTrue(result.IsSuccess);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public async Task LoginAsync_WhenValid_ReturnAccessToken()
        {
            //Arrange 
            var user = ReturnUser();
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
            var userId = await _userRepo.AddAsync(user);

            var login = new User
            {
                Email = user.Email,
                PasswordHash = Password
            };
            //Act 
            var result = await _authService.LoginAsync(login);

            //Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.AccessToken);
        }
        #endregion

        #region Negative Part

        [TestMethod]
        public async Task RegisterAsync_WhenDuplicateUser_ReturnFalse()
        {
            //Arrange 
            var user = ReturnUser();
            await _userRepo.AddAsync(user);

            //Act 

            var result = await _authService.RegisterAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);

        }

        [TestMethod]
        public async Task LoginAsync_WhenUserNotFound_ReturnFalse()
        {
            //Arrange 
            var user = ReturnUser();

            //Act 
            var result = await _authService.LoginAsync(user);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);

        }

        [TestMethod]
        public async Task LoginAsync_WhenInCorrectPassword_ReturnFalse()
        {
            //Arrange 
            var user = ReturnUser();
            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);
            var userId = await _userRepo.AddAsync(user);

            var login = new User
            {
                Email = user.Email,
                PasswordHash = Password + "iobhw" // pass wrong password 
            };
            //Act 
            var result = await _authService.LoginAsync(login);

            //Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsNotNull(result.Error);

        }

        #endregion


        #region Helper 

        public User ReturnUser()
        {
            var rand = new Random();
            var user = new User
            {
                Email = $"testuser{rand.Next(1000, 9999)}@gmail.com",
                IsActive = true,
                Role = "User",
                PasswordHash = Password
            };

            return user;

        }
        #endregion
    }
}
