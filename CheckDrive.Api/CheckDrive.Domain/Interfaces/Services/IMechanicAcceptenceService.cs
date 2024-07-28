﻿using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IMechanicAcceptanceService
    {
        Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesForMechanicAsync(MechanicAcceptanceResourceParameters resourceParameters);
        Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesAsync(MechanicAcceptanceResourceParameters resourceParameters);
        Task<MechanicAcceptanceDto?> GetMechanicAcceptenceByIdAsync(int id);
        Task<MechanicAcceptanceDto> CreateMechanicAcceptenceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto);
        Task<MechanicAcceptanceDto> UpdateMechanicAcceptenceAsync(MechanicAcceptanceForUpdateDto acceptanceForUpdateDto);
        Task DeleteMechanicAcceptenceAsync(int id);
        Task<byte[]> MonthlyExcelData(int year, int month);
    }
}
