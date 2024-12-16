using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/drivers")]
[ApiController]
public class DriversController : ControllerBase
{
    private readonly IDriverService _driverService;

    public DriversController(IDriverService driverService)
    {
        _driverService = driverService ?? throw new ArgumentNullException(nameof(driverService));
    }

    [HttpGet]
    public async Task<ActionResult<List<DriverDto>>> GetAvailableDriversAsync([FromQuery] DriverQueryParameters queryParameters)
    {
        var drivers = await _driverService.GetAsync(queryParameters);

        return Ok(drivers);
    }

    [HttpPost("reviews")]
    public async Task<IActionResult> CreateReviewConfirmation(DriverReviewConfirmationDto confirmationDto)
    {
        await _driverService.CreateReviewConfirmation(confirmationDto);

        return NoContent();
    }
}
