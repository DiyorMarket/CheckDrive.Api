using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/debts")]
[ApiController]
public class DebtController : ControllerBase
{
    private readonly IDebtService _debtService;

    public DebtController(IDebtService debtService)
    {
        _debtService = debtService ?? throw new ArgumentNullException(nameof(debtService));
    }

    [HttpGet]
    public async Task<ActionResult<List<DebtDto>>> GetAsync([FromQuery] 
    DebtQueryParametrs queryParametrs)
    {
        var debts = await _debtService.GetAsync(queryParametrs);

        return Ok(debts);
    }

    [HttpGet("{id}", Name = nameof(GetDebtByIdAsync))]
    public async Task<ActionResult<DebtDto>> GetDebtByIdAsync(int id)
    {
        var debt = await _debtService.GetByIdAsync(id);

        return Ok(debt);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DebtDto>> UpdateAsync([FromRoute] int id,
        [FromBody] UpdateDebtDto debt)
    {
        if (id != debt.Id)
        {
            return BadRequest($"Route id: {id} does not match with body id: {debt.Id}.");
        }

        var updatedDebt = await _debtService.UpdateAsync(debt);

        return Ok(updatedDebt);
    }
}
