using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Domain.Entities;
using CheckDrive.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheckDrive.Infrastructure.Helpers;

internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;

    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public string GenerateToken(Employee employee, IList<string> roles)
    {
        var claims = GetClaims(employee, roles);

        var signingKey = GetSigningKey();
        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpiresInHours),
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

    private static List<Claim> GetClaims(Employee employee, IList<string> roles)
    {
        var claims = new List<Claim>()
        {
            new (ClaimTypes.PrimarySid, employee.AccountId),
            new (ClaimTypes.NameIdentifier, employee.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}
