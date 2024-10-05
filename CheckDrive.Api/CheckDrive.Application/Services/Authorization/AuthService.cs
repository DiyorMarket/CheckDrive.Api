using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using CheckDrive.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Services.Authorization;

internal sealed class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, UserManager<IdentityUser> userManager)
    {
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        if (loginDto is null)
        {
            throw new ArgumentNullException(nameof(loginDto));
        }

        var user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user is null)
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }
       
        var roles = await _userManager.GetRolesAsync(user);

        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return token;
    }
}
