using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Employee employee, IList<string> roles);
}
