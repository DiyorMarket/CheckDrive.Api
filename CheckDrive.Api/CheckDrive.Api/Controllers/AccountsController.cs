using CheckDrive.Application.Constants;
using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountsController(IAccountService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountDto>>> GetAsync(EmployeePosition? position)
    {
        var accounts = await _service.GetAsync(position);

        return Ok(accounts);
    }

    [HttpGet("{id}", Name = nameof(GetAccountByIdAsync))]
    public async Task<ActionResult<AccountDto>> GetAccountByIdAsync(string id)
    {
        var account = await _service.GetByIdAsync(id);

        return Ok(account);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Manager},{Roles.Administrator}")]
    public async Task<ActionResult<AccountDto>> CreateAsync([FromBody] CreateAccountDto account)
    {
        var createdAccount = await _service.CreateAsync(account);

        return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountDto>> UpdateAsync([FromRoute] string id, [FromBody] UpdateAccountDto account)
    {
        if (id != account.Id)
        {
            return BadRequest($"Route id: {id} does not match with body id: {account.Id}.");
        }

        var updatedAccount = await _service.UpdateAsync(account);

        return Ok(updatedAccount);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        await _service.DeleteAsync(id);

        return NoContent();
    }
}
