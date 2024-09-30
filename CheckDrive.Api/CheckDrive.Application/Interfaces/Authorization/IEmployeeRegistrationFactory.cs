using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;
public interface IEmployeeRegistrationFactory
{
    Task CreateEmployee(RegisterDto registerDto, IdentityUser user);
}
