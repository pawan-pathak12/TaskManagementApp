using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Service;
using TaskManagmentSystem.API.Repositories.InMemory;
using TaskManagmentSystem.API.Services;

namespace TaskMangmentSystem.Test.Unit.Common
{
    public class AuthServiceTestBase
    {

        protected IAuthService _authService = null!;
        protected InMemoryUserRepo _userRepo = null!;
        protected Mock<IJwtTokenService> _tokenServiceMock = null!;
        protected Mock<IConfiguration> ConfigMock { get; private set; } = null!;

        protected PasswordHasher<User> _passwordHasher = null!;
        protected string Password = "TestingUsder!!11";

        [TestInitialize]
        public void TestInt()
        {
            var dbContext = new InMemoryDb();
            _userRepo = new InMemoryUserRepo(dbContext);
            _passwordHasher = new PasswordHasher<User>();

            //pass mock inmemeory configuration 
            //        _tokenService = new JwtTokenService();
            ConfigMock = new Mock<IConfiguration>();

            var jwtSectionMock = new Mock<IConfigurationSection>();

            jwtSectionMock.Setup(u => u["Key"]).Returns("this-is-your-very-secure-jwt-secret-key-later_can-be-changed12345");
            jwtSectionMock.Setup(u => u["Issuer"]).Returns("TestIssuer");
            jwtSectionMock.Setup(u => u["Audience"]).Returns("MyTestAudience");
            jwtSectionMock.Setup(u => u["ExpiresInMinutes"]).Returns("15");

            ConfigMock.Setup(c => c.GetSection("Jwt")).Returns(jwtSectionMock.Object);

            // Mock IJwtTokenService (instead of real one)
            _tokenServiceMock = new Mock<IJwtTokenService>();

            // Default setups - you can override in specific tests
            _tokenServiceMock.Setup(t => t.CreateToken(It.IsAny<User>()))
                .Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.fake.jwt.token");

            _authService = new AuthService(
                _userRepo,
                _tokenServiceMock.Object);
        }
    }
}
