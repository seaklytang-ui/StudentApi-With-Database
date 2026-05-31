using Microsoft.IdentityModel.Tokens;
using StudentApi.Data;
using StudentApi.DTOs;
using StudentApi.Interfaces;
using StudentApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(
            AppDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<TokenResponseDto> LoginAsync(
            LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.Username == dto.Username &&
                x.Password == dto.Password);

            if (user == null)
            {
                throw new Exception("Invalid username or password");
            }

            var accessToken = GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(
            string refreshToken)
        {
            var tokenEntity = _context.RefreshTokens
                .FirstOrDefault(x =>
                    x.Token == refreshToken &&
                    !x.IsRevoked &&
                    x.ExpiryDate > DateTime.UtcNow);

            if (tokenEntity == null)
            {
                throw new Exception("Invalid refresh token");
            }

            var user = await _context.Users.FindAsync(
                tokenEntity.UserId);

            var newAccessToken = GenerateJwtToken(user);

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}