using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.OperatorReview;

public record CreateOperatorReviewDto(
    Guid ReviewerId,
    Guid DriverId,
    int OilMarkId,
    string? Notes,
    double InitialOilAmount,
    double OilRefillAmount,
    bool IsApprovedByReviewer)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
