namespace CheckDrive.Application.DTOs.Review;

public abstract record CreateReviewDtoBase(
    Guid ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer);
