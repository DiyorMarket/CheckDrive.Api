using CheckDrive.Application.DTOs.Account;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Interfaces;

public interface IAccountService
{
    Task<List<AccountDto>> GetAsync(EmployeePosition? position);
    Task<AccountDto> GetByIdAsync(string id);
    Task<AccountDto> CreateAsync(CreateAccountDto account);
    Task<AccountDto> UpdateAsync(UpdateAccountDto account);
    Task DeleteAsync(string id);
}
