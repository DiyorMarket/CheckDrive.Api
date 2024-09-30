using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;
public interface IEmployeeFactory
{
    Task CreateEmployee(RegisterDto registerDto, IdentityUser user);
}
