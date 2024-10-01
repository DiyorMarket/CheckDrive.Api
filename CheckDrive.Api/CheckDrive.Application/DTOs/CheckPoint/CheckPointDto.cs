﻿using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed record CheckPointDto(
    int Id,
    DateTime StartDate,
    CheckPointStatus Status,
    CheckPointStage Stage,
    DoctorReviewDto DoctorReview,
    MechanicHandoverReviewDto? MechanicHandover,
    OperatorReviewDto? OperatorReview,
    MechanicAcceptanceReviewDto? MechanicAcceptance,
    DispatcherReviewDto? DispatcherReview,
    DebtDto? Debt);