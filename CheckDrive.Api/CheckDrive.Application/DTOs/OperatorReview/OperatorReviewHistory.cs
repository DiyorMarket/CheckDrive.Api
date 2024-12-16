namespace CheckDrive.Application.DTOs.OperatorReview;

public sealed record OperatorReviewHistory(
    int CheckPointId,
    int DriverId,
    string DriverName,
    int OilMarkId,
    string OilMarkName,
    string? Notes,
    DateTime Date,
    decimal InitialOilAmount,
    decimal OilRefillAmount);
