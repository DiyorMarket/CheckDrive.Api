using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.OperatorReview;

public sealed record CreateOperatorReviewDto(
    int CheckPointId,
    Guid ReviewerId,
    int OilMarkId,
    string? Notes,
    bool IsApprovedByReviewer,
    decimal InitialOilAmount,
    decimal OilRefillAmount)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
