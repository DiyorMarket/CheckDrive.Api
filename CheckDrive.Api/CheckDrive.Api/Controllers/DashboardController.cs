using CheckDrive.Application.DTOs.Dashboard;
using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/dashboard")]
[ApiController]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
    }

    [HttpGet]
    public async Task<ActionResult<DashboardDto>> GetDashboardAsync()
    {
        var dashboard = await _dashboardService.GetDashboardAsync();

        return Ok(dashboard);
    }
}
