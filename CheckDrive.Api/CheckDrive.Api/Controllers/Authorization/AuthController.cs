using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Authorization;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService
            ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("register/employee")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Administrator}")]
    public async Task<IActionResult> Register(RegisterDto registerUser)
    {
        await _authService.RegisterEmployeeAsync(registerUser);
        return Created();
    }

    [HttpPost("register/admin")]
    [Authorize(Roles = $"{Roles.Administrator}")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto registerUser)
    {
        await _authService.RegisterAdministratorAsync(registerUser);
        return Created();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginUser)
    {
        var token = await _authService.LoginAsync(loginUser);
        return Ok(token);
    }
}
