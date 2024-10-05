using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser user, IList<string> roles);
}
