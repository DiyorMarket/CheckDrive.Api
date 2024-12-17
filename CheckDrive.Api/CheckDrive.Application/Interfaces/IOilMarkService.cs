using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface IOilMarkService
{
    Task<List<OilMarkDto>> GetAllAsync(OilMarkQueryParameters queryParameters);
    Task<OilMarkDto> GetByIdAsync(int id);
    Task<OilMarkDto> CreateAsync(CreateOilMarkDto oilMark);
    Task<OilMarkDto> UpdateAsync(UpdateOilMarkDto oilMark);
    Task DeleteAsync(int id);
}
