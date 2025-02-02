using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/debts")]
[ApiController]
public class DebtController(IDebtService debtService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<DebtDto>>> GetAsync([FromQuery]
    DebtQueryParametrs queryParametrs)
    {
        var debts = await debtService.GetAsync(queryParametrs);

        return Ok(debts);
    }

    [HttpGet("{id}", Name = nameof(GetDebtByIdAsync))]
    public async Task<ActionResult<DebtDto>> GetDebtByIdAsync(int id)
    {
        var debt = await debtService.GetByIdAsync(id);

        return Ok(debt);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DebtDto>> UpdateAsync([FromRoute] int id,
        [FromBody] DebtDto debt)
    {
        if (id != debt.Id)
        {
            return BadRequest($"Route id: {id} does not match with body id: {debt.Id}.");
        }

        var updatedDebt = await debtService.UpdateAsync(debt);

        return Ok(updatedDebt);
    }
}
