using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Interfaces.Authorization;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser user, IList<string> roles);
}
