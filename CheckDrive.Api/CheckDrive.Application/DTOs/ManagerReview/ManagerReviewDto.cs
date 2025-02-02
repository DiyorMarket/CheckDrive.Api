using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.ManagerReview;

public sealed record ManagerReviewDto(
    int Id,
    int CheckPointId,
    int ReviewerId,
    string ReviewerName,
    int DriverId,
    string DriverName,
    string? Notes,
    DateTime Date,
    int FinalMileage,
    decimal DebtAmount,
    int InitialMillage,
    decimal FuelConsumptionAmount,
    decimal RemainingFuelAmount)
    : ReviewDtoBase(
        CheckPointId: CheckPointId,
        ReviewerId: ReviewerId,
        ReviewerName: ReviewerName,
        DriverId: DriverId,
        DriverName: DriverName,
        Notes: Notes,
        Date: Date,
        Type: ReviewType.ManagerReview);
