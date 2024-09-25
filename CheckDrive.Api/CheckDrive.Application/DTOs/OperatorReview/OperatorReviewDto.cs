using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.OperatorReview;

public sealed record OperatorReviewDto(
    int CheckPointId,
    Guid ReviewerId,
    string ReviewerName,
    Guid DriverId,
    string DriverName,
    int OilMarkId,
    string OilMarkName,
    string? Notes,
    DateTime Date,
    ReviewStatus Status,
    decimal InitialOilAmount,
    decimal OilRefillAmount)
    : ReviewDtoBase(
        CheckPointId: CheckPointId,
        ReviewerId: ReviewerId,
        ReviewerName: ReviewerName,
        DriverId: DriverId,
        DriverName: DriverName,
        Notes: Notes,
        Date: Date,
        Status: Status,
        Type: ReviewType.Operator);
