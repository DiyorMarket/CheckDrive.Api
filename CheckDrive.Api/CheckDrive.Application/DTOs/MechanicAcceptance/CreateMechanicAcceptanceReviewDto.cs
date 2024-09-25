using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicAcceptance;

public sealed record CreateMechanicAcceptanceReviewDto(
    int CheckPointId,
    Guid ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer,
    int FinalMileage,
    decimal RemainingFuelAmount)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
