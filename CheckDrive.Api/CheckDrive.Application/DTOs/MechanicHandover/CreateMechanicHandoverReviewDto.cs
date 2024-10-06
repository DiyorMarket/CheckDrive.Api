using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public sealed record CreateMechanicHandoverReviewDto(
    int CheckPointId,
    int CarId,
    int DriverId,
    int ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer,
    int InitialMileage)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
