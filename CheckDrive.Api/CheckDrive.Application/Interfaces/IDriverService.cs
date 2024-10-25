using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Interfaces;

public interface IDriverService
{
    Task<List<DriverDto>> GetAvailableDriversAsync(CheckPointStage stage);
}
