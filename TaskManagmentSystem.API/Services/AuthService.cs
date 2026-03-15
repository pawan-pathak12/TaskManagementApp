using Microsoft.AspNetCore.Identity;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Repositories;
using TaskManagmentSystem.API.Interfaces.Service;
using TaskManagmentSystem.API.Results;

namespace TaskManagmentSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _tokenService;

        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(IUserRepository userRepository, IJwtTokenService tokenService)
        {
            this._userRepository = userRepository;
            this._tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResult> RegisterAsync(User user)
        {
            var userExists = await _userRepository.UserExistsAsync(user.Email);
            if (userExists)
            {
                return AuthResult.Failure("user already exists");
            }
            var newUser = new User
            {
                Email = user.Email,
                IsActive = true,
                Role = "User",
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, user.PasswordHash);
            await _userRepository.AddAsync(newUser);

            return AuthResult.Success("User Added successfully");
        }

        public async Task<AuthResult> LoginAsync(User user)
        {
            var storedUser = await _userRepository.GetByEmailAsync(user.Email);

            if (user != null)
            {
                return AuthResult.Failure("Invalid credentials");
            }
            var result = _passwordHasher.VerifyHashedPassword(storedUser, storedUser.PasswordHash, user.PasswordHash);
            if (result == PasswordVerificationResult.Failed)
            {
                return AuthResult.Failure("Invalid credentials");
            }
            var token = _tokenService.CreateToken(user);

            return AuthResult.Success("Verified", token);
        }


    }
}
