using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/checkPoints")]
[ApiController]
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
}
