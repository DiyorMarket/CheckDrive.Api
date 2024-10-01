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

    [HttpGet("{id}", Name = nameof(GetAccountByIdAsync))]
    public async Task<ActionResult<AccountDto>> GetAccountByIdAsync(string id)
    {
        var account = await _service.GetByIdAsync(id);

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAsync([FromBody] CreateAccountDto account)
    {
        var createdAccount = await _service.CreateAsync(account);

        return CreatedAtAction(nameof(GetAccountByIdAsync), new { id = createdAccount.Id }, createdAccount);
    }
}
