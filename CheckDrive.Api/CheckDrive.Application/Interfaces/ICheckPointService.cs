using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Domain.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface ICheckPointService
{
    Task<List<CheckPointDto>> GetAsync(CheckPointQueryParameters queryParameters);
    Task<CheckPointDto> GetCurrentByDriverIdAsync(int driverId);
    Task<CheckPointDto> GetByIdAsync(int id);
    Task CancelAsync(int id);
}
