using CheckDrive.Application.DTOs.Identity;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;
public interface IAuthService
{
    Task<string> LoginAsync(LoginDto login);
    Task RegisterAdministratorAsync(RegisterDto registerUser);
    Task RegisterAsync(RegisterDto registerUser);
}
