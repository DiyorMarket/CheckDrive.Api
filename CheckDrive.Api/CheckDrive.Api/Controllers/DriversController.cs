using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
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
    public async Task<ActionResult<List<DriverDto>>> GetAvailableDriversAsync(CheckPointStage? stage)
    {
        var drivers = await _driverService.GetAvailableDriversAsync(stage ?? CheckPointStage.DoctorReview);

        return Ok(drivers);
    }
}
