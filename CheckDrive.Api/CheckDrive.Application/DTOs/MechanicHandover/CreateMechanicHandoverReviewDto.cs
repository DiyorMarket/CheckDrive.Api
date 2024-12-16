namespace CheckDrive.Application.DTOs.MechanicHandover;

public sealed record CreateMechanicHandoverReviewDto(
    int CheckPointId,
    int CarId,
    int MechanicId,
    string? Notes,
    int InitialMileage);
