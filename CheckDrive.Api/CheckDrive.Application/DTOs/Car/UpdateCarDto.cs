using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Car;

public record UpdateCarDto(
    int Id,
    int OilMarkId,
    string Model,
    string Number,
    int ManufacturedYear,
    decimal Mileage,
    decimal CurrentMonthMileage,
    decimal CurrentYearMileage,
    decimal MonthlyDistanceLimit,
    decimal YearlyDistanceLimit,
    decimal CurrentMonthFuelConsumption,
    decimal CurrentYearFuelConsumption,
    decimal MonthlyFuelConsumptionLimit,
    decimal YearlyFuelConsumptionLimit,
    decimal AverageFuelConsumption,
    decimal FuelCapacity,
    decimal RemainingFuel,
    CarStatus Status);
