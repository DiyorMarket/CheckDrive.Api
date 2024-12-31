using CheckDrive.Application.DTOs.Auth;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CheckDrive.Application.Services.Auth;

internal sealed class AuthService : IAuthService
{
    private readonly ICheckDriveDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenHandler _tokenHandler;

    public AuthService(
        ICheckDriveDbContext context,
        UserManager<IdentityUser> userManager,
        ITokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TokenDto> LoginAsync(LoginDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await _context.Employees
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Account.UserName == request.UserName);

        if (employee is null || !await _userManager.CheckPasswordAsync(employee.Account, request.Password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }

        var roles = await _userManager.GetRolesAsync(employee.Account);
        var accessToken = _tokenHandler.GenerateAccessToken(employee, roles);
        var refreshToken = _tokenHandler.GenerateRefreshToken();
        var tokenEntity = CreateTokenEntity(employee, refreshToken);

        _context.RefreshTokens.Add(tokenEntity);
        await _context.SaveChangesAsync();

        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<TokenDto> RefreshTokenAsync(RefreshTokenRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var token = await GetAndValidateRefreshTokenAsync(request.RefreshToken);
        var employee = await GetAndValidateEmployeeAsync(token.UserId);
        var roles = await _userManager.GetRolesAsync(token.User);
        var accessToken = _tokenHandler.GenerateAccessToken(employee, roles);
        var refreshToken = _tokenHandler.GenerateRefreshToken();

        await ReplaceRefreshTokenAsync(employee, request.RefreshToken, refreshToken);

        return new TokenDto(accessToken, refreshToken);
    }

    private async Task<RefreshToken> GetAndValidateRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (token is null || token.ExpiresAtUtc < DateTime.UtcNow || token.IsRevoked)
        {
            throw new SecurityTokenException("Refresh token is invalid.");
        }

        return token;
    }

    private async Task<Employee> GetAndValidateEmployeeAsync(string accountId)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(x => x.AccountId == accountId);

        if (employee is null)
        {
            throw new EntityNotFoundException($"Employee with account id: {accountId} is not found");
        }

        return employee;
    }

    private async Task ReplaceRefreshTokenAsync(Employee employee, string oldToken, string newToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == oldToken);

        if (token is not null)
        {
            token.IsRevoked = true;
        }

        var tokenEntity = CreateTokenEntity(employee, newToken);

        _context.RefreshTokens.Add(tokenEntity);
        await _context.SaveChangesAsync();
    }

    private static RefreshToken CreateTokenEntity(Employee employee, string token) =>
        new()
        {
            Token = token,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            IsRevoked = false,
            UserId = employee.AccountId,
            User = employee.Account,
        };
}
