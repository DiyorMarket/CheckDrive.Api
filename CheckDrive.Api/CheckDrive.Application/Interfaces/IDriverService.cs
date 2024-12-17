using CheckDrive.Application.DTOs.Driver;

namespace CheckDrive.Application.Interfaces;

public interface IDriverService
{
    Task<List<DriverDto>> GetAvailableDriversAsync();
    Task CreateReviewConfirmation(DriverReviewConfirmationDto confirmation);
}
