namespace CheckDrive.Application.DTOs.DoctorReview;

public sealed record CreateDoctorReviewDto(
    int DriverId,
    int DoctorId,
    string? Notes,
    bool IsHealthy);
