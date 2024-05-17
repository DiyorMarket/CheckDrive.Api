﻿using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.ApiContracts.Mechanic;

namespace CheckDrive.Domain.Interfaces.Services
{
    public interface IMechanicService
    {
        Task<GetBaseResponse<MechanicDto>> GetMechanicesAsync(MechanicResourceParameters resourceParameters);
        Task<MechanicDto?> GetMechanicByIdAsync(int id);
        Task<MechanicDto> CreateMechanicAsync(MechanicForCreateDto mechanicForCreate);
        Task DeleteMechanicAsync(int id);
    }
}
