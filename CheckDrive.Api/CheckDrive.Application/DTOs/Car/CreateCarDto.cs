namespace CheckDrive.Application.DTOs.Car;

public record CreateCarDto(
    string Model,
    string Color,
    string Number,
    int ManufacturedYear,
    int Mileage,
    int CurrentMonthMileage,
    int YearlyDistanceLimit,
    decimal AverageFuelConsumption,
    decimal FuelCapacity,
    decimal RemainingFuel);
