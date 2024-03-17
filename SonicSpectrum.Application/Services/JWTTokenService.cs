using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SonicSpectrum.Application.UserSessions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityManagerServerApi.Services
{
    public class JwtTokenService(IConfiguration _config)
    {
        public string CreateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id !),
                new Claim(ClaimTypes.Name, user.Name !),
                new Claim(ClaimTypes.Email, user.Email !),
                new Claim(ClaimTypes.Role, user.Role !)
            };
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
