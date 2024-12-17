using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public sealed record DriverReviewDto(
    string? Notes,
    string ReviewerName,
    DateTime Date,
    ReviewType Type);
