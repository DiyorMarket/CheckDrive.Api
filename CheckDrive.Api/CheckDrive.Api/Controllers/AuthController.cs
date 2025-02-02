using CheckDrive.Application.DTOs.Auth;
using CheckDrive.Application.DTOs.Identity;
using CheckDrive.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto request)
    {
        var token = await authService.LoginAsync(request);

        return Ok(token);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshTokenAsync([FromBody] RefreshTokenRequest request)
    {
        var token = await authService.RefreshTokenAsync(request);

        return Ok(token);
    }
}
