using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers
{
    [Authorize(Policy = "AdminOrOperator")]
    [ApiController]
    [Route("api/oilMarks")]
    public class OilMarksController : Controller
    {
        private readonly IOilMarkService _oilMarkService;

        public OilMarksController(IOilMarkService oilMarkService)
        {
            _oilMarkService = oilMarkService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OilMarkDto>>> GetOilMarksAsync(
        [FromQuery] OilMarkResourceParameters oilMarkResource)
        {
            var marks = await _oilMarkService.GetMarksAsync(oilMarkResource);

            return Ok(marks);
        }

        [HttpGet("{id}", Name = "GetOilMarkById")]
        public async Task<ActionResult<OilMarkDto>> GetOilMarkByIdAsync(int id)
        {
            var marks = await _oilMarkService.GetMarkByIdAsync(id);

            if (marks is null)
                return NotFound($"Oil Mark with id: {id} does not exist.");

            return Ok(marks);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] OilMarkForCreateDto forCreateDto)
        {
            var createdOilMark = await _oilMarkService.CreateMarkAsync(forCreateDto);

            return CreatedAtAction("GetOilMarkById", new { createdOilMark.Id }, createdOilMark);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] OilMarkForUpdateDto forUpdateDto)
        {
            if (id != forUpdateDto.Id)
            {
                return BadRequest(
                    $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
            }

            var updateMark = await _oilMarkService.UpdateMarkAsync(forUpdateDto);

            return Ok(updateMark);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _oilMarkService.DeleteMarkAsync(id);

            return NoContent();
        }
    }
}
