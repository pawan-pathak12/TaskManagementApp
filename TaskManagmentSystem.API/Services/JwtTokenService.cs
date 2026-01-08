using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagmentSystem.API.Entities;
using TaskManagmentSystem.API.Interfaces.Service;

namespace TaskManagmentSystem.API.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string CreateToken(User user)
        {
            return CreateJwtToken(user);
        }

        private string CreateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            //1. claim

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim (JwtRegisteredClaimNames.Email , user .Email !),
                new Claim(ClaimTypes.Role , user.Role),
                new Claim (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())
           };

            //2. 
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //3. create and return token 
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims, expires: DateTime.UtcNow.AddMinutes(
                    double.Parse(jwtSettings["ExpiresInMinutes"]!)
                    ),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}