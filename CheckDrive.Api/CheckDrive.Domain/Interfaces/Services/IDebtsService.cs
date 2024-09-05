using CheckDrive.ApiContracts.Debts;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IDebtsService
    {
        Task<GetBaseResponse<DebtsDto>> GetDebtsAsync(DebtsResourceParameters resourceParameters);
        Task<DebtsDto?> GetDebtByIdAsync(int id);
        Task<DebtsDto> CreateDebtAsync(DebtsForCreateDto accountForCreate);
        Task<DebtsDto> UpdateDebtAsync(DebtsForUpdateDto accountForUpdate);
        Task DeleteDebtAsync(int id);
    }
}
