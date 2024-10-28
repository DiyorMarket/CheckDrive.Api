using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/oilMarks")]
[ApiController]
public class OilMarksController : ControllerBase
{
    private readonly IOilMarkService _oilMarkService;

    public OilMarksController(IOilMarkService oilMarkService)
    {
        _oilMarkService = oilMarkService ?? throw new ArgumentNullException(nameof(oilMarkService));
    }

    [HttpGet]
    public async Task<ActionResult<List<OilMarkDto>>> GetAsync()
    {
        var oilMarks = await _oilMarkService.GetAllAsync();

        return Ok(oilMarks);
    }
}
