using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface ICarService
{
    Task<List<CarDto>> GetAllAsync(CarQueryParameters queryParameters);
    Task<List<CarDto>> GetAvailableCarsAsync();
    Task<CarDto> GetByIdAsync(int id);
    Task<CarDto> CreateAsync(CreateCarDto car);
    Task<CarDto> UpdateAsync(UpdateCarDto car);
    Task DeleteAsync(int id);
}
