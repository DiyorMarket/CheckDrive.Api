using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheckDrive.Infrastructure.Helpers;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options )
    {
        _options = options.Value 
        ?? throw new ArgumentNullException(nameof(options));
    }

    public string GenerateToken(IdentityUser user,
        IList<string> roles)
    {
        var claims = GetClaims(user, roles);

        var signingKey = GetSigningKey();
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresHours),
            signingCredentials: signingKey);

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
