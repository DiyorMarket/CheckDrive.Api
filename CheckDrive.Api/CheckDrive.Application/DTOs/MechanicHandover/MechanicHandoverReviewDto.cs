using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public sealed record MechanicHandoverReviewDto(
    int CheckPointId,
    int ReviewerId,
    string ReviewerName,
    int DriverId,
    string DriverName,
    string CarModel,
    string? Notes,
    DateTime Date,
    ReviewStatus Status,
    int InitialMileage)
    : ReviewDtoBase(
        CheckPointId: CheckPointId,
        ReviewerId: ReviewerId,
        ReviewerName: ReviewerName,
        DriverId: DriverId,
        DriverName: DriverName,
        Notes: Notes,
        Date: Date,
        Status: Status,
        Type: ReviewType.MechanicHandover);
