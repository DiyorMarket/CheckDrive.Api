namespace CheckDrive.Application.DTOs.DispatcherReview;

public sealed record DispatcherReviewHistoryDto(
    int CheckPointId,
    int DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    int FinalMileage,
    decimal FuelConsumptionAmount,
    decimal RemainingFuelAmount);
