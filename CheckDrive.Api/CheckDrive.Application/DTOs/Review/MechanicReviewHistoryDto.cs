namespace CheckDrive.Application.DTOs.Review;

public sealed record MechanicReviewHistoryDto(
    int ReviewId,
    int DriverId,
    string DriverName,
    string? Notes,
    DateTime Date);
