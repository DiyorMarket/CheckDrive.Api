using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public sealed record CreateMechanicHandoverReviewDto(
    int CheckPointId,
    int CarId,
    Guid ReviewerId,
    string? Notes,
    int InitialMileage,
    bool IsApprovedByReviewer)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
