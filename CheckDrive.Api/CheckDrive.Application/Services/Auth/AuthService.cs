using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Auth;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Auth;

internal sealed class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICheckDriveDbContext _context;

    public AuthService(
        IJwtTokenGenerator jwtTokenGenerator,
        UserManager<IdentityUser> userManager,
        ICheckDriveDbContext context)
    {
        _jwtTokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<string> LoginAsync(LoginDto request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await _context.Employees.Include(x => x.Account).FirstOrDefaultAsync(x => x.Account.UserName == request.UserName);

        if (employee is null || !await _userManager.CheckPasswordAsync(employee.Account, request.Password))
        {
            throw new InvalidLoginAttemptException("Invalid email or password");
        }

        var roles = await _userManager.GetRolesAsync(employee.Account);
        var token = _jwtTokenGenerator.GenerateToken(employee, roles);

        return token;
    }
}
