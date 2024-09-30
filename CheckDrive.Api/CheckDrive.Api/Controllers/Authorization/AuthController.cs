using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Authorization;
[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService
        ?? throw new ArgumentNullException(nameof(authService));

    [HttpPost("register/employee")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Administrator}")]
    public async Task<IActionResult> Register(RegisterDto registerUser)
    {
        // Register the new user with a position (role)
        await _authService.RegisterEmployeeAsync(registerUser);
        return Accepted();
    }

    [HttpPost("register/admin")]
    [Authorize(Roles = $"{Roles.Administrator}")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto registerUser)
    {
        // Register the new user with a role administrator
        await _authService.RegisterAdministratorAsync(registerUser);
        return Accepted();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginUser)
    {
        //Login user and gets token
        var token = await _authService.LoginAsync(loginUser);
        return Ok(token);
    }
}
