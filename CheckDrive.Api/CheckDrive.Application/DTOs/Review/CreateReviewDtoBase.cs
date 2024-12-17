namespace CheckDrive.Application.DTOs.Review;

public abstract record CreateReviewDtoBase(
    int ReviewerId,
    string? Notes);
