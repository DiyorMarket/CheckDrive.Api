using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces;
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
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto request)
    {
        var token = await _authService.LoginAsync(request);
        return Ok(token);
    }
}
