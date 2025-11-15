using Microsoft.IdentityModel.Tokens;
using Storefront.UserService.Entities;
using Storefront.UserService.Infrastructure.Repository;
using Storefront.UserService.Models.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Storefront.UserService.Services
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly JwtOptions _jwtOptions;
        private readonly List<string> _adminUsers;
        public JwtProvider(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _jwtOptions = _configuration.GetSection("JwtSettings").Get<JwtOptions>()!;
            _adminUsers = _configuration.GetSection("Users:Admin").Get<List<string>>() ?? new List<string>();
        }

        public int JwtRefreshExpirationInHours { get => _jwtOptions.JwtRefreshExpirationInHours; }

        public string Genarate(User user, out DateTime expiresAt)
        {
            var userRole = _adminUsers.Contains(user.Email ?? string.Empty) ? "Admin" : "User";
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, $"{user.FirstName} { user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", user.Id.ToString())
            };

            var signingCredentials = new SigningCredentials( new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretToken)),SecurityAlgorithms.HmacSha256);
            expiresAt = DateTime.UtcNow.AddHours(_jwtOptions.JwtExpirationInMinutes);
            var token = new JwtSecurityToken(_jwtOptions.Issuer, _jwtOptions.Audience, claims, null, expiresAt, signingCredentials);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }

        public string GenarateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
