using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Driver;

public sealed record DriverDto(
    int Id,
    int? AssignedCarId,
    string FullName,
    DriverStatus Status);
