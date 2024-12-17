namespace CheckDrive.Application.DTOs.DoctorReview;

public sealed record DoctorReviewDto(
    int CheckPointId,
    int DoctorId,
    string DoctorName,
    int DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    bool IsHealthy);
