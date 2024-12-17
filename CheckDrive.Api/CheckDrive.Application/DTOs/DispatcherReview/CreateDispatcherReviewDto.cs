namespace CheckDrive.Application.DTOs.DispatcherReview;

public sealed record CreateDispatcherReviewDto(
    int CheckPointId,
    int DispatcherId,
    string? Notes,
    int FinalMileage,
    decimal FuelConsumptionAmount,
    decimal RemainingFuelAmount);
