using CheckDrive.Application.DTOs.Identity;

namespace CheckDrive.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto request);
}
