using CheckDrive.Application.DTOs.Car;

namespace CheckDrive.Application.Interfaces;

public interface ICarService
{
    Task<List<CarDto>> GetAvailableCarsAsync();
}
