﻿using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.DoctorReview;

public sealed record DoctorReviewDto(
    int CheckPointId,
    Guid DriverId,
    string DriverName,
    Guid ReviewerId,
    string ReviewerName,
    string? Notes,
    DateTime Date,
    ReviewStatus Status)
    : ReviewDtoBase(
        CheckPointId: CheckPointId,
        DriverId: DriverId,
        DriverName: DriverName,
        ReviewerId: ReviewerId,
        ReviewerName: ReviewerName,
        Date: Date,
        Notes: Notes,
        Status: Status,
        Type: ReviewType.Doctor);