using CheckDrive.Application.DTOs.Employee;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/employees")]
[ApiController]
public class EmployeesController(IEmployeeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAsync([FromQuery] EmployeeQueryParameters queryParameters)
    {
        var accounts = await service.GetAsync(queryParameters);

        return Ok(accounts);
    }

    [HttpGet("{id}", Name = nameof(GetEmployeeByIdAsync))]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        var account = await service.GetByIdAsync(id);

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateAsync([FromBody] CreateEmployeeDto account)
    {
        var createdAccount = await service.CreateAsync(account);

        return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateAsync([FromRoute] int id, [FromBody] UpdateEmployeeDto account)
    {
        if (id != account.Id)
        {
            return BadRequest($"Route id: {id} does not match with body id: {account.Id}.");
        }

        var updatedAccount = await service.UpdateAsync(account);

        return Ok(updatedAccount);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await service.DeleteAsync(id);

        return NoContent();
    }
}
