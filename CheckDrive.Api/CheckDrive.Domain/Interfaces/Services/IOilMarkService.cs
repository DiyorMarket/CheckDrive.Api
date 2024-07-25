using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IOilMarkService
    {
        Task<GetBaseResponse<OilMarkDto>> GetMarksAsync(OilMarkResourceParameters resourceParameters);
        Task<OilMarkDto?> GetMarkByIdAsync(int id);
        Task<OilMarkDto> CreateMarkAsync(OilMarkForCreateDto markForCreate);
        Task<OilMarkDto> UpdateMarkAsync(OilMarkForUpdateDto markForUpdate);
        Task DeleteMarkAsync(int id);
    }
}
