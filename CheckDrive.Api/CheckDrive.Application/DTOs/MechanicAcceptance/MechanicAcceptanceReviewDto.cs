using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.MechanicAcceptance;

public sealed record MechanicAcceptanceReviewDto(
    int CheckPointId,
    int MechanicId,
    string MechanicName,
    string? Notes,
    DateTime Date,
    int FinalMileage,
    bool IsCarInGoodCondition,
    ReviewStatus Status);
