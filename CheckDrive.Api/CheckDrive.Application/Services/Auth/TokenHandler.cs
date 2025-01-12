using CheckDrive.Application.Configurations;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CheckDrive.Application.Services.Auth;

internal sealed class TokenHandler : ITokenHandler
{
    private readonly JwtOptions _options;

    public TokenHandler(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateAccessToken(Employee employee, IList<string> roles)
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

    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomNumbers);

        var token = Convert.ToBase64String(randomNumbers);

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
