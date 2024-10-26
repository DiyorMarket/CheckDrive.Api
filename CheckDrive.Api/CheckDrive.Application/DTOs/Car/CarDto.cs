using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Car;

public sealed record CarDto(
    int Id,
    string Model,
    string Color,
    string Number,
    int ManufacturedYear,
    int Mileage,
    int YearlyDistanceLimit,
    decimal AverageFuelConsumption,
    decimal FuelCapacity,
    decimal RemainingFuel,
    CarStatus Status);
