using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Auth;

internal sealed class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, UserManager<IdentityUser> userManager)
    {
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> LoginAsync(LoginDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return token;
    }
}
