using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public sealed record ReviewConfirmationDto(
    int CheckPointId,
    ReviewType ReviewType,
    string Message);
