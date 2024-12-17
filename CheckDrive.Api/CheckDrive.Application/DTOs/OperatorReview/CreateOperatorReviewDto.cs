namespace CheckDrive.Application.DTOs.OperatorReview;

public sealed record CreateOperatorReviewDto(
    int CheckPointId,
    int OperatorId,
    int OilMarkId,
    string? Notes,
    decimal InitialOilAmount,
    decimal OilRefillAmount);
