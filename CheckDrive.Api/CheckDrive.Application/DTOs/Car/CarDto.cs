using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Car;

public record CarDto(
    int Id,
    int? OilMarkId,
    string? OilMarkName,
    string Model,
    string Number,
    int ManufacturedYear,
    int Mileage,
    int CurrentMonthMileage,
    int CurrentYearMileage,
    int MonthlyDistanceLimit,
    int YearlyDistanceLimit,
    decimal CurrentMonthFuelConsumption,
    decimal CurrentYearFuelConsumption,
    decimal MonthlyFuelConsumptionLimit,
    decimal YearlyFuelConsumptionLimit,
    decimal AverageFuelConsumption,
    decimal FuelCapacity,
    decimal RemainingFuel,
    CarStatus Status);
