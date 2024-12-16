﻿namespace CheckDrive.Application.DTOs.DispatcherReview;

public sealed record DispatcherReviewDto(
    int CheckPointId,
    int DispatcherId,
    string DispatcherName,
    string? Notes,
    DateTime Date,
    int FinalMileage,
    decimal FuelConsumptionAmount,
    decimal RemainingFuelAmount);
