using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/checkPoints")]
[ApiController]
public class CheckPointsController(ICheckPointService service) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<CheckPointDto>>> GetCheckPointsAsync([FromQuery] CheckPointQueryParameters queryParameters)
    {
        var checkPoints = await service.GetAsync(queryParameters);

        return Ok(checkPoints);
    }

    [HttpGet("drivers/{driverId:int}/current")]
    public async Task<ActionResult<CheckPointDto>> GetCurrentCheckPointByDriverIdAsync(int driverId)
    {
        var checkPoint = await service.GetCurrentByDriverIdAsync(driverId);

        return Ok(checkPoint);
    }

    [HttpPut("{id:int}/cancel")]
    public async Task<ActionResult<CheckPointDto>> CancelCheckPoint(int id)
    {
        await service.CancelAsync(id);

        return NoContent();
    }
}
