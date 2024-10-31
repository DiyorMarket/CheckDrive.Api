using CheckDrive.Application.DTOs.OilMark;

namespace CheckDrive.Application.Interfaces;

public interface IOilMarkService
{
    Task<List<OilMarkDto>> GetAllAsync();
}
