﻿using AutoMapper;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class MechanicAcceptanceService : IMechanicAcceptanceService
{
    private readonly ICheckDriveDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public MechanicAcceptanceService(
        ICheckDriveDbContext context,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MechanicAcceptanceReviewDto> CreateAsync(CreateMechanicAcceptanceReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var mechanic = await GetAndValidateMechanicAsync(review.ReviewerId);

        var entity = CreateReviewEntity(checkPoint, mechanic, review);
        var createdReview = _context.MechanicAcceptances.Add(entity).Entity;

        UpdateCheckPoint(checkPoint, review);
        UpdateCar(checkPoint, review);

        await _context.SaveChangesAsync();

        var dto = _mapper.Map<MechanicAcceptanceReviewDto>(createdReview);

        return dto;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.OperatorReview)
            .Include(x => x.MechanicHandover)
            .ThenInclude(mh => mh!.Car)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Stage != CheckPointStage.OperatorReview)
        {
            throw new InvalidOperationException(
                $"Check Point Stage should be in Operator Review to start Mechanic Acceptance Review. Check Point Stage: {checkPoint.Stage}.");
        }

        if (checkPoint.OperatorReview is null)
        {
            throw new InvalidOperationException(
                "Operator review should not be null in order to start Mechanic Acceptance Review.");
        }

        if (checkPoint.OperatorReview.Status != ReviewStatus.Approved)
        {
            throw new InvalidOperationException(
                $"Operator Review should be approved before starting Mechanic Acceptance Review. " +
                $"Current operator review status: {checkPoint.OperatorReview.Status}");
        }

        return checkPoint;
    }

    private async Task<User> GetAndValidateMechanicAsync(Guid mechanicId)
    {
        var mechanic = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == mechanicId);

        if (mechanic is null)
        {
            throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.");
        }

        if (mechanic.Position != EmployeePosition.Mechanic)
        {
            throw new InvalidOperationException("Only Mechanics can perform Mechanic Acceptance Review.");
        }

        return mechanic;
    }

    private static MechanicAcceptance CreateReviewEntity(
        CheckPoint checkPoint,
        User mechanic,
        CreateMechanicAcceptanceReviewDto review)
    {
        var entity = new MechanicAcceptance
        {
            CheckPoint = checkPoint,
            Mechanic = mechanic,
            FinalMileage = review.FinalMileage,
            RemainingFuelAmount = review.RemainingFuelAmount,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer,
        };

        return entity;
    }

    private static void UpdateCheckPoint(CheckPoint checkPoint, CreateMechanicAcceptanceReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        checkPoint.Stage = CheckPointStage.MechanicAcceptance;

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }
    }

    private static void UpdateCar(CheckPoint checkPoint, CreateMechanicAcceptanceReviewDto review)
    {
        if (checkPoint.MechanicHandover is null)
        {
            throw new InvalidOperationException($"Mechanic Handover in Check Point cannot be null.");
        }

        var car = checkPoint.MechanicHandover.Car;

        if (car.FuelCapacity < review.RemainingFuelAmount)
        {
            throw new FuelAmountExceedsCarCapacityException($"Remaining amount: {review.RemainingFuelAmount} exceeds Capacity: {car.FuelCapacity}.");
        }
        car.RemainingFuel = review.RemainingFuelAmount;

        if (review.FinalMileage < car.Mileage)
        {
            throw new InvalidMileageException(
                $"Final mileage ({review.FinalMileage}) cannot be less than the current milea of a car ({car.Mileage}).");
        }
        car.Mileage = review.FinalMileage;
    }
}