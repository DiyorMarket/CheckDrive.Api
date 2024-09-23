using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public record CreateMechanicHandoverDto(
    Guid ReviewerId,
    Guid DriverId,
    int CarId,
    string? Notes,
    int InitialMileage,
    bool IsApprovedByReviewer)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
