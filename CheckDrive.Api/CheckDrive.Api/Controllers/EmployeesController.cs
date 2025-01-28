using CheckDrive.Application.DTOs.Employee;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/employees")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAsync([FromQuery] EmployeeQueryParameters queryParameters)
    {
        var accounts = await _service.GetAsync(queryParameters);

        return Ok(accounts);
    }

    [HttpGet("{id}", Name = nameof(GetEmployeeByIdAsync))]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeByIdAsync(int id)
    {
        var account = await _service.GetByIdAsync(id);

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateAsync([FromBody] CreateEmployeeDto account)
    {
        var createdAccount = await _service.CreateAsync(account);

        return CreatedAtAction(nameof(GetEmployeeByIdAsync), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EmployeeDto>> UpdateAsync([FromRoute] int id, [FromBody] UpdateEmployeeDto account)
    {
        if (id != account.Id)
        {
            return BadRequest($"Route id: {id} does not match with body id: {account.Id}.");
        }

        var updatedAccount = await _service.UpdateAsync(account);

        return Ok(updatedAccount);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
