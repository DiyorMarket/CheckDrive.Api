using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Interfaces;
public interface IDebtService
{
    Task<List<DebtDto>> GetAsync(DebtQueryParametrs queryParameters);
    Task<DebtDto> GetByIdAsync(int id);
    Task<DebtDto> UpdateAsync(DebtDto debt);
    Task DeleteAsync(int id);
}
