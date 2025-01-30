using CheckDrive.Domain.Enums;
using System;

namespace CheckDrive.Application.DTOs.Car;

public class CarDto
{
    public int Id { get; set; }
    public int OilMarkId { get; set; }
    public required string Model { get; set; }
    public required string Number { get; set; }
    public int ManufacturedYear { get; set; }
    public decimal Mileage { get; set; }
    public decimal? CurrentMonthMileage { get; set; }
    public decimal? CurrentYearMileage { get; set; }
    public decimal MonthlyDistanceLimit { get; set; }
    public decimal YearlyDistanceLimit { get; set; }
    public decimal? CurrentMonthFuelConsumption { get; set; }
    public decimal? CurrentYearFuelConsumption { get; set; }
    public decimal MonthlyFuelConsumptionLimit { get; set; }
    public decimal YearlyFuelConsumptionLimit { get; set; }
    public decimal AverageFuelConsumption { get; set; }
    public decimal FuelCapacity { get; set; }
    public decimal RemainingFuel { get; set; }
    public CarStatus Status { get; set; }
}
