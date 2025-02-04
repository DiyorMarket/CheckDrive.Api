using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/oilMarks")]
[ApiController]
public class OilMarksController(IOilMarkService oilMarkService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<OilMarkDto>>> GetAsync([FromQuery] OilMarkQueryParameters queryParameters)
    {
        var oilMarks = await oilMarkService.GetAllAsync(queryParameters);

        return Ok(oilMarks);
    }

    [HttpGet("{id:int}", Name = "GetOilMarkById")]
    public async Task<ActionResult<OilMarkDto>> GetByIdAsync(int id)
    {
        var oilMark = await oilMarkService.GetByIdAsync(id);

        return oilMark;
    }

    [HttpPost]
    public async Task<ActionResult<OilMarkDto>> CreateAsync(CreateOilMarkDto oilMark)
    {
        var createdOilMark = await oilMarkService.CreateAsync(oilMark);

        return CreatedAtAction("GetOilMarkById", oilMark, new { id = createdOilMark.Id });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<OilMarkDto>> UpdateAsync(int id, UpdateOilMarkDto oilMark)
    {
        if (id != oilMark.Id)
        {
            return BadRequest($"Route parameter id: {id} does not match with body parameter id: {oilMark.Id}.");
        }

        var updatedOilMark = await oilMarkService.UpdateAsync(oilMark);

        return Ok(updatedOilMark);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await oilMarkService.DeleteAsync(id);

        return NoContent();
    }
}
