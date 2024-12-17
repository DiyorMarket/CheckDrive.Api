using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.OperatorReview;

public sealed record OperatorReviewDto(
    int CheckPointId,
    int OperatorId,
    string OperatorName,
    string? Notes,
    DateTime Date,
    decimal InitialOilAmount,
    decimal OilRefillAmount,
    ReviewStatus Status,
    OilMarkDto OilMark);
