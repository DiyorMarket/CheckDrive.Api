using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;

namespace CheckDrive.Application.Services;

internal sealed class DriverService : IDriverService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public DriverService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<DriverDto>> GetAsync(DriverQueryParameters queryParameters)
    {
        var query = GetQuery(queryParameters);

        var drivers = await query
            .AsNoTracking()
            .ProjectTo<DriverDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return drivers;
    }

    public async Task CreateReviewConfirmation(DriverReviewConfirmationDto confirmation)
    {
        ArgumentNullException.ThrowIfNull(confirmation);

        var checkPoint = await GetAndValidateCheckPointAsync(confirmation.CheckPointId);

        if (confirmation.IsAccepted)
        {
            AcceptReview(checkPoint, confirmation);
        }
        else
        {
            RejectReview(checkPoint, confirmation);
        }

        await _context.SaveChangesAsync();
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Car)
            .Include(x => x.OperatorReview)
            .Include(x => x.MechanicAcceptance)
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new InvalidOperationException($"Check Point with id: {checkPointId} is not found.");
        }

        if (checkPoint.Status != CheckPointStatus.InProgress)
        {
            throw new InvalidOperationException(
                $"Driver can perform review confirmation only for 'In Progress' Check Point. Current check point status: {checkPoint.Status}");
        }

        return checkPoint;
    }

    private IQueryable<Driver> GetQuery(DriverQueryParameters queryParameters)
    {
        var query = _context.Drivers.AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(x => x.FirstName.Contains(queryParameters.SearchText)
                || x.LastName.Contains(queryParameters.SearchText)
                || x.Patronymic.Contains(queryParameters.SearchText)
                || x.Passport != null && x.Passport.Contains(queryParameters.SearchText)
                || x.Address != null && x.Address.Contains(queryParameters.SearchText));
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(x => x.Status == queryParameters.Status.Value);
        }

        return query;
    }

    private static void AcceptReview(CheckPoint checkPoint, DriverReviewConfirmationDto confirmation)
    {
        if (confirmation.ReviewType == ReviewType.MechanicHandover)
        {
            ChangeCarStatusToBusy(checkPoint);
        }

        UpdateReviewStatus(checkPoint, confirmation);

        if (confirmation.IsAccepted)
        {
            checkPoint.Stage = GetStage(confirmation);
            checkPoint.Status = CheckPointStatus.InProgress;
        }
    }

    private static void RejectReview(CheckPoint checkPoint, DriverReviewConfirmationDto confirmation)
    {
        UpdateReviewStatus(checkPoint, confirmation);
    }

    private static void UpdateReviewStatus(CheckPoint checkPoint, DriverReviewConfirmationDto confirmation)
    {
        var status = confirmation.IsAccepted ? ReviewStatus.Approved : ReviewStatus.Rejected;

        switch (confirmation.ReviewType)
        {
            case ReviewType.MechanicHandover:
                checkPoint.MechanicHandover.Status = status;
                break;
            case ReviewType.OperatorReview:
                checkPoint.OperatorReview.Status = status;
                break;
            case ReviewType.MechanicAcceptance:
                checkPoint.MechanicAcceptance.Status = status;
                break;
        }
    }

    private static void ChangeCarStatusToBusy(CheckPoint checkPoint)
    {
        if (checkPoint.MechanicHandover is null || checkPoint.MechanicHandover.Car is null)
        {
            throw new InvalidOperationException("Cannot update car status for check point without Mechanic Handover.");
        }

        var car = checkPoint.MechanicHandover.Car;
        if (car.Status != CarStatus.Free)
        {
            throw new InvalidOperationException($"Cannot change car status with id ({car.Id}) to busy when car is not Free ({car.Status}).");
        }

        car.Status = CarStatus.Busy;
    }

    private static CheckPointStage GetStage(DriverReviewConfirmationDto review)
        => review.ReviewType switch
        {
            ReviewType.MechanicHandover => CheckPointStage.MechanicHandover,
            ReviewType.OperatorReview => CheckPointStage.OperatorReview,
            ReviewType.MechanicAcceptance => CheckPointStage.MechanicAcceptance,
            _ => throw new ArgumentOutOfRangeException($"Could not resolve next stage for review type: {review.ReviewType}.")
        };
}
