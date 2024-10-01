using CheckDrive.Application.DTOs.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto login);
    Task RegisterAsync(RegisterDto registerUser);
}
