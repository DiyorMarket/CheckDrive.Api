namespace CheckDrive.Application.DTOs.Ride;

public record RideDetailsDto(
    int CheckPointId,
    int CarId,
    int FinalMileage,
    decimal FuelConsumptionAmount,
    decimal RemainingFuelAmount);
