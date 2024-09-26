using AutoMapper;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class MechanicHandoverService : IMechanicHandoverService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public MechanicHandoverService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var mechanic = await GetAndValidateMechanicAsync(review.ReviewerId);
        var checkPoint = await GetAndValidateCheckPointAsync(review.CheckPointId);
        var car = await GetAndValidateCarAsync(review.CarId);

        UpdateCheckPoint(checkPoint, review);
        UpdateCar(car, review);

        var reviewEntity = CreateReviewEntity(review, mechanic, car, checkPoint);

        var createdReview = _context.MechanicHandovers.Add(reviewEntity).Entity;
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<MechanicHandoverReviewDto>(createdReview);

        return dto;
    }

    private async Task<User> GetAndValidateMechanicAsync(Guid mechanicId)
    {
        var mechanic = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == mechanicId && x.Position == EmployeePosition.Mechanic);

        if (mechanic is null)
        {
            throw new EntityNotFoundException($"Mechanic with id: {mechanicId} is not found.");
        }

        return mechanic;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Where(x => x.Id == checkPointId)
            .Where(x => x.Stage == CheckPointStage.DoctorReview)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .FirstOrDefaultAsync();

        if (checkPoint == null)
        {
            throw new InvalidOperationException($"Cannot start mechanic review without doctor's review present.");
        }

        if (checkPoint.DoctorReview.Status != ReviewStatus.Approved)
        {
            throw new InvalidOperationException($"Cannot start car handover review when Doctor review is not approved.");
        }

        return checkPoint;
    }

    private async Task<Car> GetAndValidateCarAsync(int carId)
    {
        var car = await _context.Cars
            .FirstOrDefaultAsync(x => x.Id == carId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {carId} is not found.");
        }

        if (car.Status != CarStatus.Free)
        {
            throw new UnavailableCarException($"Car with id: {carId} is not available for handover.");
        }

        return car;
    }

    private static MechanicHandover CreateReviewEntity(
        CreateMechanicHandoverReviewDto review,
        User mechanic,
        Car car,
        CheckPoint checkPoint)
    {
        var entity = new MechanicHandover()
        {
            CheckPoint = checkPoint,
            Car = car,
            Mechanic = mechanic,
            InitialMileage = review.InitialMileage,
            Notes = review.Notes,
            Date = DateTime.UtcNow,
            Status = review.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer
        };

        return entity;
    }

    private static void UpdateCar(Car car, CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(car);

        if (review.InitialMileage < car.Mileage)
        {
            throw new InvalidMileageException(
                $"Initial mileage ({review.InitialMileage}) cannot be less than current mileage of car: {car.Mileage}.");
        }

        car.Mileage = review.InitialMileage;
        car.Status = CarStatus.Busy;
    }

    private static void UpdateCheckPoint(CheckPoint checkPoint, CreateMechanicHandoverReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        checkPoint.Stage = CheckPointStage.MechanicHandover;

        if (!review.IsApprovedByReviewer)
        {
            checkPoint.Status = CheckPointStatus.InterruptedByReviewerRejection;
        }
    }
}
