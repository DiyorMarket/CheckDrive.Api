using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public abstract record ReviewDtoBase(
    int CheckPointId,
    Guid ReviewerId,
    string ReviewerName,
    Guid DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    ReviewStatus Status,
    ReviewType Type);
