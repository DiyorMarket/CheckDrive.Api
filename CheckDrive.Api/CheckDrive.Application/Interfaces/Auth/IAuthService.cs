using CheckDrive.Application.DTOs.Auth;
using CheckDrive.Application.DTOs.Identity;

namespace CheckDrive.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<TokenDto> LoginAsync(LoginDto request);
    Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request);
}
