using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public sealed record MechanicHandoverReviewDto(
    int CheckPointId,
    int MechanicId,
    string MechanicName,
    string? Notes,
    DateTime Date,
    int InitialMileage,
    ReviewStatus Status);
