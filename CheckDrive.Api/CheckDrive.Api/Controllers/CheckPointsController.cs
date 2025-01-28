using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/checkPoints")]
[ApiController]
[Authorize]
public class CheckPointsController : ControllerBase
{
    private readonly ICheckPointService _service;

    public CheckPointsController(ICheckPointService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<List<CheckPointDto>>> GetCheckPointsAsync([FromQuery] CheckPointQueryParameters queryParameters)
    {
        var checkPoints = await _service.GetCheckPointsAsync(queryParameters);

        return Ok(checkPoints);
    }

    [HttpGet("drivers/{driverId:int}/current")]
    public async Task<ActionResult<CheckPointDto>> GetCurrentCheckPointByDriverIdAsync(int driverId)
    {
        var checkPoint = await _service.GetCurrentCheckPointByDriverIdAsync(driverId);

        return Ok(checkPoint);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<ActionResult<CheckPointDto>> CancelCheckPoint(int id)
    {
        await _service.CancelCheckPointAsync(id);

        return NoContent();
    }
}
