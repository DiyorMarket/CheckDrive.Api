namespace CheckDrive.Application.DTOs.MechanicAcceptance;

public sealed record CreateMechanicAcceptanceReviewDto(
    int CheckPointId,
    int MechanicId,
    string? Notes,
    int FinalMileage,
    bool IsCarInGoodCondition);
