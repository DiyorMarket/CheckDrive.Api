﻿using CheckDrive.ApiContracts.Car;
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
        Task<IEnumerable<CarHistoryDto>> GetCarHistories(int year, int month);
    }
}
