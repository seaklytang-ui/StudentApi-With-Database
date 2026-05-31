using StudentApi.DTOs;

namespace StudentApi.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto> LoginAsync(LoginDto dto);
        Task<TokenResponseDto> RefreshTokenAsync(
            string refreshToken);
    }
}
