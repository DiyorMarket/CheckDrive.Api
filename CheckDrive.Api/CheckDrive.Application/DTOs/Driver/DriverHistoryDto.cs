using CheckDrive.Application.DTOs.Car;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Driver;

public sealed record DriverHistoryDto(
    int DriverId,
    int TravelledDistance,
    decimal FuelConsumptionAmount,
    decimal DebtAmount,
    CheckPointStatus Status,
    DateTime StartDate,
    CarDto Car);
