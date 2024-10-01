using CheckDrive.Domain.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheckDrive.Application.Services.Authorization;
public sealed class JwtHandler(IOptions<JwtOptions> options,
    UserManager<IdentityUser>userManager)
{
    private readonly JwtOptions _options = options.Value 
        ?? throw new ArgumentNullException(nameof(options));
    private readonly UserManager<IdentityUser> _userManager = userManager
        ?? throw new ArgumentNullException(nameof(userManager));

    public async Task<string> GenerateTokenAsync(IdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = GetClaims(user, roles);

        var signingKey = GetSigningKey();
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
            signingCredentials: signingKey );

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    private SigningCredentials GetSigningKey()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var signingKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return signingKey;
    }

    private static List<Claim> GetClaims(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
