using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface IDriverService
{
    Task<List<DriverDto>> GetAsync(DriverQueryParameters queryParameters);
    Task CreateReviewConfirmation(DriverReviewConfirmationDto confirmation);
}
