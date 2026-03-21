using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Enums;
using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskMangmentSystem.Test.API.DataBuilder
{
    public class TestDataBuilder
    {
        public IUserRepository _userRepository = null!;
        public ITokenService _tokenService = null!;
        public ITaskRepository _taskRepository = null!;
        private readonly PasswordHasher<User> passwordHasher = null!;
        public string Password = "TestUser11!";
        private Random rand = new Random();

        public TestDataBuilder(IServiceProvider serviceProvider)
        {
            passwordHasher = new PasswordHasher<User>();
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _tokenService = serviceProvider.GetRequiredService<ITokenService>();
            _taskRepository = serviceProvider.GetRequiredService<ITaskRepository>();
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
        public async Task<TodoItem?> CreateAndReturnTask(int userId)
        {
            var task = new TodoItem
            {
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                Title = "Testing",
                Status = TasksStatus.Pending,
                Description = "hello world"
            };
            var taskId = await _taskRepository.AddAsync(task);
            return await _taskRepository.GetByIdAsync(taskId, userId);
        }

        public string GenerateAndReturnToken(User user)
        {
            return _tokenService.CreateToken(user);
        }
    }
}
