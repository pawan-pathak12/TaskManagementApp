using Microsoft.AspNetCore.Mvc;
using TaskManagmentSystem.API.DTOs;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskManagmentSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }


        #region Register EndPoint
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto registerRequest)
        {
            var user = new User
            {
                Email = registerRequest.Email,
                PasswordHash = registerRequest.Password
            };
            var result = await _authService.RegisterAsync(user);
            if (!result.IsSuccess)
            {
                return BadRequest($"Error : {result.Error}");
            }

            return Ok("User Registered successfully");
        }
        #endregion

        #region Login EndPoint
        [HttpPost("login")]

        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequest)
        {
            var user = new User
            {
                Email = loginRequest.Email,
                PasswordHash = loginRequest.Password
            };

            var result = await _authService.LoginAsync(user);
            if (!result.IsSuccess)
            {

                return BadRequest($" Error : {result.Error}");
            }

            return Ok(new { accesstoken = result.AccessToken });
        }
        #endregion
    }

}