using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public record ReviewDtoBase(
    string? Notes,
    DateTime Date,
    ReviewStatus Status);
