using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface ICarService
    {
        Task<GetBaseResponse<CarDto>> GetCarsAsync(CarResourceParameters resourceParameters);
        Task<CarDto?> GetCarByIdAsync(int id);
        Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate);
        Task<CarDto> UpdateCarAsync(CarForUpdateDto carForUpdate);
        Task DeleteCarAsync(int id);
        Task<byte[]> MonthlyExcelData(PropertyForExportFile propertyForExportFile);
        Task<GetBaseResponse<CarHistoryDto>> GetCarHistories(CarResourceParameters carResource);
    }
}
