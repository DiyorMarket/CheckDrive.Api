using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.DispatcherReview;

public sealed record DispatcherReviewDto(
    int CheckPointId,
    int ReviewerId,
    string ReviewerName,
    int DriverId,
    string DriverName,
    int OperatorId,
    string OperatorName,
    string CarModel,
    decimal AvarageFuelConsumption,
    decimal InitialRemainingFuel,
    int InitialMilage,
    int FinalMilage,
    decimal OilRefillAmount,
    decimal RemainingFuel,
    string? Notes,
    DateTime Date,
    ReviewStatus Status,
    decimal? FuelConsumptionAdjustment,
    decimal? DistanceTravelledAdjustment)
    : ReviewDtoBase(
        CheckPointId: CheckPointId,
        ReviewerId: ReviewerId,
        ReviewerName: ReviewerName,
        DriverId: DriverId,
        DriverName: DriverName,
        Notes: Notes,
        Date: Date,
        Status: Status,
        Type: ReviewType.Dispatcher);
