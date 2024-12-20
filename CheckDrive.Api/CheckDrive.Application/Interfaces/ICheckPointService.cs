﻿using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Domain.QueryParameters;

namespace CheckDrive.Application.Interfaces;

public interface ICheckPointService
{
    Task<List<CheckPointDto>> GetCheckPointsAsync(CheckPointQueryParameters queryParameters);
    Task<CheckPointDto> GetCurrentCheckPointByDriverIdAsync(int driverId);
    Task CancelCheckPointAsync(int id);
}
