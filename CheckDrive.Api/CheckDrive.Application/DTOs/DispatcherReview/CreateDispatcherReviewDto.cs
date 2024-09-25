using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.DispatcherReview;

public sealed record CreateDispatcherReviewDto(
    int CheckPointId,
    Guid ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer,
    decimal? FuelConsumptionAdjustment,
    int? DistanceTravelledAdjustment)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
