using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.DoctorReview;

public sealed record CreateDoctorReviewDto(
    int CheckPointId,
    int DriverId,
    int ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer)
    : CreateReviewDtoBase(
        ReviewerId: ReviewerId,
        Notes: Notes,
        IsApprovedByReviewer: IsApprovedByReviewer);
