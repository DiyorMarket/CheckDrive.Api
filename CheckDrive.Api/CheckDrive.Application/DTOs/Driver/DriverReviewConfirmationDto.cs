using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Driver;

public sealed record DriverReviewConfirmationDto(
    int CheckPointId,
    ReviewType ReviewType,
    bool IsAccepted,
    string? Notes);
