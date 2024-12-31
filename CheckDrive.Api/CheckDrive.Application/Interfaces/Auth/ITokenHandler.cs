using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Interfaces.Auth;

public interface ITokenHandler
{
    string GenerateAccessToken(Employee employee, IList<string> roles);
    string GenerateRefreshToken();
}
