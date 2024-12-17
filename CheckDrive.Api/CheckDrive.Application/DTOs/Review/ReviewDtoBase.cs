using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public abstract record ReviewDtoBase(
    int CheckPointId,
    int ReviewerId,
    string ReviewerName,
    int DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    ReviewType Type);
