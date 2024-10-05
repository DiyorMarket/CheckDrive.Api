using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.DTOs.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto login);
}
