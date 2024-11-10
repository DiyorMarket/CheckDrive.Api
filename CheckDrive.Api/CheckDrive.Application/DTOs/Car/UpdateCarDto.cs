using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Car;

public record UpdateCarDto(
    int Id,
    string Model,
    string Color,
    string Number,
    int ManufacturedYear,
    int Mileage,
    int CurrentMonthMileage,
    int YearlyDistanceLimit,
    decimal AverageFuelConsumption,
    decimal FuelCapacity,
    decimal RemainingFuel,
    CarStatus Status);
