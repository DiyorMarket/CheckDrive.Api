using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers.Authorization;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register/employee")]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Administrator}")]
    public async Task<IActionResult> Register(RegisterDto registerUser)
    {
        // Register the new user with a position (role)
        await _authService.RegisterAsync(registerUser);

        return Accepted();
    }

    [HttpPost("register/admin")]
    [Authorize(Roles = $"{Roles.Administrator}")]
    public async Task<IActionResult> RegisterAdmin(RegisterDto registerUser)
    {
        // Register the new user with a position (role)
        await _authService.RegisterAdministratorAsync(registerUser);

        return Accepted();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginUser)
    {
        var token = await _authService.LoginAsync(loginUser);

        return Ok(token);
    }
}
