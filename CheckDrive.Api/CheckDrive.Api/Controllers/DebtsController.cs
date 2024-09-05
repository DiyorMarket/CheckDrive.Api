using CheckDrive.ApiContracts.Debts;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.ResourceParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers
{
    public class DebtsController(IDebtsService debtsService) : Controller
    {
        private readonly IDebtsService _debtsService = debtsService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtsDto>>> GetDebtsAsync(
        [FromQuery] DebtsResourceParameters debtsResource)
        {
            var debts = await _debtsService.GetDebtsAsync(debtsResource);

            return Ok(debts);
        }

        [HttpGet("{id}", Name = "GetDebtById")]
        public async Task<ActionResult<DebtsDto>> GetDebtByIdAsync(int id)
        {
            var debt = await _debtsService.GetDebtByIdAsync(id);

            if (debt is null)
                return NotFound($"Debt with id: {id} does not exist.");

            return Ok(debt);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] DebtsForCreateDto forCreateDto)
        {
            var createdDebt = await _debtsService.CreateDebtAsync(forCreateDto);

            return null;//CreatedAtAction("GetDebtById", new { id = createdDebt.Id }, createdDebt);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] DebtsForUpdateDto forUpdateDto)
        {
            //if (id != forUpdateDto.Id)
            //{
            //    return BadRequest(
            //        $"Route id: {id} does not match with parameter id: {forUpdateDto.Id}.");
            //}

            var updateDebts = await _debtsService.UpdateDebtAsync(forUpdateDto);

            return Ok(updateDebts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _debtsService.DeleteDebtAsync(id);

            return NoContent();
        }
    }
}
