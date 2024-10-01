using AutoMapper;
using CheckDrive.Application.DTOs.Account;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class AccountService : IAccountService
{
    private readonly ICheckDriveDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public AccountService(ICheckDriveDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<List<AccountDto>> GetAsync()
    {
        var accounts = await _context.Employees
            .AsNoTracking()
            .Include(x => x.Account)
            .ToListAsync();

        var dtos = _mapper.Map<List<AccountDto>>(accounts);

        return dtos;
    }

    public async Task<AccountDto> GetByIdAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var account = await _context.Employees
            .AsNoTracking()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.AccountId == id);

        if (account is null)
        {
            throw new EntityNotFoundException($"Account with id: {id} is not found.");
        }

        var dto = _mapper.Map<AccountDto>(account);

        return dto;
    }

    public async Task<AccountDto> CreateAsync(CreateAccountDto account)
    {
        ArgumentNullException.ThrowIfNull(account);

        var user = _mapper.Map<IdentityUser>(account);
        var result = await _userManager.CreateAsync(user, account.PasswordConfirm);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Could not create user account.");
        }

        var employee = _mapper.Map<Employee>(account);
        employee.AccountId = user.Id;

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        employee.Account = user;
        var dto = _mapper.Map<AccountDto>(employee);

        return dto;
    }

    // Update Employee & Accunt
    public Task<AccountDto> UpdateAsync(UpdateAccountDto account)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(string id)
    {
        var account = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (account is null)
        {
            throw new EntityNotFoundException($"User account with id: {id} is not found.");
        }

        _context.Users.Remove(account);
        await _context.SaveChangesAsync();
    }
}
