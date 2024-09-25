using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.MechanicAcceptance;

public sealed record MechanicAcceptanceReviewDto(
    int CheckPointId,
    Guid ReviewerId,
    string ReviewerName,
    Guid DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    ReviewStatus Status,
    int FinalMileage,
    decimal RemainingFuelAmount)
    : ReviewDtoBase(
        CheckPointId,
        ReviewerId,
        ReviewerName,
        DriverId,
        DriverName,
        Notes,
        Date,
        Status,
        ReviewType.MechanicAcceptance);
