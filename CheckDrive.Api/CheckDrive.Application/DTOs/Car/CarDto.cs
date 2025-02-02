using CheckDrive.Domain.Enums;
using System;

namespace CheckDrive.Application.DTOs.Car;

public record CarDto(
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
