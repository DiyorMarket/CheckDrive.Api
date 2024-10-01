using CheckDrive.Application.DTOs.Account;

namespace CheckDrive.Application.Interfaces;

public interface IAccountService
{
    Task<List<AccountDto>> GetAsync();
    Task<AccountDto> GetByIdAsync(string id);
    Task<AccountDto> CreateAsync(CreateAccountDto account);
    Task<AccountDto> UpdateAsync(UpdateAccountDto account);
    Task DeleteAsync(string id);
}
