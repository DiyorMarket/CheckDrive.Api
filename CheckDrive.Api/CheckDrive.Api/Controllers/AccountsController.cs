using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.Interfaces;
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
    public async Task<ActionResult<List<AccountDto>>> GetAsync()
    {
        var accounts = await _service.GetAsync();

        return Ok(accounts);
    }

    [HttpGet("{id}", Name = "GetByIdAsync")]
    public async Task<ActionResult<AccountDto>> GetAccountByIdAsync(string id)
    {
        var account = await _service.GetByIdAsync(id);

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAsync([FromBody] CreateAccountDto account)
    {
        var createdAccount = await _service.CreateAsync(account);

        return CreatedAtAction("GetByIdAsync", new { id = createdAccount.Id }, createdAccount);
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
