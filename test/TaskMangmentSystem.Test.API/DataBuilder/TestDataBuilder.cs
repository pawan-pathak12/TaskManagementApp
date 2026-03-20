using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskMangmentSystem.Test.API.DataBuilder
{
    public class TestDataBuilder
    {
        public IUserRepository _userRepository = null!;
        public ITokenService _tokenService = null!;
        private readonly PasswordHasher<User> passwordHasher = null!;
        public string Password = "TestUser11!";
        private Random rand = new Random();

        public TestDataBuilder(IServiceProvider serviceProvider)
        {
            passwordHasher = new PasswordHasher<User>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _tokenService = serviceProvider.GetRequiredService<ITokenService>();
        }

        public async Task<User?> CreateAndReturnUser()
        {
            var user = new User
            {
                Email = $"testuser{rand.Next(1000, 9999)}@gmail.com",
                IsActive = true,
                Role = "Admin",
            };
            user.PasswordHash = passwordHasher.HashPassword(user, Password);

            var userId = await _userRepository.AddAsync(user);

            var userData = await _userRepository.GetByIdAsync(userId);

            return new User
            {
                Id = userData.Id,
                Email = userData.Email,
                IsActive = userData.IsActive,
                Role = userData.Role
            };
        }

        public string GenerateAndReturnToken(User user)
        {
            return _tokenService.CreateToken(user);
        }
    }
}
