using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.DoctorReview;

public sealed record CreateDoctorReviewDto(
    Guid DriverId,
    Guid ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
