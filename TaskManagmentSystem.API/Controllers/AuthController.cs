using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagmentSystem.API.Data;
using TaskManagmentSystem.API.DTOs;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskManagmentSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthController(IJwtTokenService jwtTokenService, ApplicationDbContext context)
        {
            this._jwtTokenService = jwtTokenService;
            this._context = context;
            _passwordHasher = new PasswordHasher<User>();
        }


        #region Register EndPoint
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExists = await _context.Users.AnyAsync(x => x.Email == registerRequest.Email);
            if (userExists)
            {
                return BadRequest("User already exists");

            }
            var user = new User
            {
                Email = registerRequest.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerRequest.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Registered successfully");
        }
        #endregion

        #region Login EndPoint
        [HttpPost("login")]

        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == loginRequest.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            //hash the enter password and compare with database 
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid credentials");
            }
            var token = _jwtTokenService.CreateToken(user);
            return Ok(token);
        }
        #endregion
    }

}
