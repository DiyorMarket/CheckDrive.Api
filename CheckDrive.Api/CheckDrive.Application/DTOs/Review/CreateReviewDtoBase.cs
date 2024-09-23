namespace CheckDrive.Application.DTOs.Review;

public record CreateReviewDtoBase(
    Guid ReviewerId,
    string? Notes,
    bool IsApprovedByReviewer);
