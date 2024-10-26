using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Domain.QueryParameters;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class CheckPointService : ICheckPointService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public CheckPointService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<CheckPointDto>> GetCheckPointsAsync(CheckPointQueryParameters queryParameters)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        var query = GetQuery(queryParameters);
        var checkPoints = await query.ToArrayAsync();
        var dtos = _mapper.Map<List<CheckPointDto>>(checkPoints);

        return dtos;
    }

    public Task<CheckPointDto> GetCheckPointsByDriverIdAsync(int driverId)
    {
        throw new NotImplementedException();
    }

    public async Task<DriverCheckPointDto> GetCurrentCheckPointByDriverIdAsync(int driverId)
    {
        var checkPoint = await _context.CheckPoints
            .AsNoTracking()
            .Where(x => x.DoctorReview != null)
            .Where(x => x.DoctorReview.DriverId == driverId)
            .Where(x => x.StartDate.Date == DateTime.UtcNow.Date)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Doctor)
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Mechanic)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Car)
            .Include(x => x.OperatorReview)
            .ThenInclude(x => x.Operator)
            .Include(x => x.MechanicAcceptance)
            .ThenInclude(x => x.Mechanic)
            .Include(x => x.DispatcherReview)
            .ThenInclude(x => x.Dispatcher)
            .FirstOrDefaultAsync();

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} does not have current active Check Point.");
        }

        var dto = MapToDto(checkPoint);

        return dto;
    }

    private IQueryable<CheckPoint> GetQuery(CheckPointQueryParameters queryParameters)
    {
        var query = _context.CheckPoints
            .AsNoTracking()
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Doctor)
            .Include(x => x.MechanicHandover)
            .ThenInclude(x => x.Mechanic)
            .Include(x => x.OperatorReview)
            .ThenInclude(x => x.Operator)
            .Include(x => x.MechanicAcceptance)
            .ThenInclude(x => x.Mechanic)
            .Include(x => x.DispatcherReview)
            .ThenInclude(x => x.Dispatcher)
            .Include(x => x.Debt)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Search))
        {
            query = query.Where(x =>
                (x.Notes != null && x.Notes.Contains(queryParameters.Search)) ||
                (x.DoctorReview.Notes != null && x.DoctorReview.Notes.Contains(queryParameters.Search)) ||
                (x.MechanicHandover != null && x.MechanicHandover.Notes != null && x.MechanicHandover.Notes.Contains(queryParameters.Search)) ||
                (x.OperatorReview != null && x.OperatorReview.Notes != null && x.OperatorReview.Notes.Contains(queryParameters.Search)) ||
                (x.MechanicAcceptance != null && x.MechanicAcceptance.Notes != null && x.MechanicAcceptance.Notes.Contains(queryParameters.Search)) ||
                (x.DispatcherReview != null && x.DispatcherReview.Notes != null && x.DispatcherReview.Notes.Contains(queryParameters.Search)));
        }

        if (queryParameters.DriverId.HasValue)
        {
            query = query.Where(x => x.DoctorReview.DriverId == queryParameters.DriverId.Value);
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(x => x.Status == queryParameters.Status.Value);
        }

        if (queryParameters.Stage.HasValue)
        {
            query = query.Where(x => x.Stage == queryParameters.Stage.Value);
        }

        if (queryParameters.Date.HasValue)
        {
            query = FilterByDate(query, queryParameters.Date.Value);
        }
        else
        {
            query = FilterByDate(query, DateFilter.Today);
        }

        return query;
    }

    private static IQueryable<CheckPoint> FilterByDate(IQueryable<CheckPoint> query, DateFilter dateFilter)
    {
        return dateFilter switch
        {
            DateFilter.Today => query.Where(x => x.StartDate.Date == DateTime.UtcNow.Date),
            DateFilter.Yesterday => query.Where(x => x.StartDate.Date == DateTime.UtcNow.AddDays(-1).Date),
            DateFilter.Week => query.Where(x => x.StartDate.Date > DateTime.UtcNow.AddDays(-7).Date),
            DateFilter.Month => query.Where(x => x.StartDate.Date > DateTime.UtcNow.AddMonths(-1).Date),
            _ => throw new ArgumentOutOfRangeException($"Date filter: {dateFilter} is not implemented yet."),
        };
    }

    private List<ReviewDtoBase> GetReviews(CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        var reviews = new List<ReviewDtoBase>();
        var doctorReview = _mapper.Map<DoctorReviewDto>(checkPoint.DoctorReview);
        reviews.Add(doctorReview);

        if (checkPoint.MechanicHandover is not null)
        {
            var mechanicHandover = _mapper.Map<MechanicHandoverReviewDto>(checkPoint.MechanicHandover);
            reviews.Add(mechanicHandover);
        }

        if (checkPoint.OperatorReview is not null)
        {
            var operatorReview = _mapper.Map<OperatorReviewDto>(checkPoint.OperatorReview);
            reviews.Add(operatorReview);
        }

        if (checkPoint.MechanicAcceptance is not null)
        {
            var mechanicAcceptance = _mapper.Map<MechanicAcceptanceReviewDto>(checkPoint.MechanicAcceptance);
            reviews.Add(mechanicAcceptance);
        }

        if (checkPoint.DispatcherReview is not null)
        {
            var dispatcherReview = _mapper.Map<DispatcherReviewDto>(checkPoint.DispatcherReview);
            reviews.Add(dispatcherReview);
        }

        return reviews;
    }

    private static DebtDto? GetDebt(CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        if (checkPoint.Debt is null)
        {
            return null;
        }

        var debtEntity = checkPoint.Debt;
        var debtDto = new DebtDto(
            CheckPointId: checkPoint.Id,
            FualAmount: debtEntity.FuelAmount,
            PaidAmount: debtEntity.PaidAmount,
            Status: debtEntity.Status);

        return debtDto;
    }

    private DriverCheckPointDto MapToDto(CheckPoint checkPoint)
    {
        ArgumentNullException.ThrowIfNull(checkPoint);

        var reviews = new List<DriverReviewDto>();
        CarDto? car = null;

        {
            var doctorReview = checkPoint.DoctorReview;
            var review = new DriverReviewDto(
                doctorReview.Notes,
                doctorReview.Doctor.FirstName + " " + doctorReview.Doctor.LastName,
                doctorReview.Date,
                ReviewType.DoctorReview,
                doctorReview.Status);
            reviews.Add(review);
        }

        if (checkPoint.MechanicHandover is not null)
        {
            var mechanicReview = checkPoint.MechanicHandover;
            var review = new DriverReviewDto(
                mechanicReview.Notes,
                mechanicReview.Mechanic.FirstName + " " + mechanicReview.Mechanic.LastName,
                mechanicReview.Date,
                ReviewType.MechanicHandover,
                mechanicReview.Status);
            reviews.Add(review);
            car = _mapper.Map<CarDto>(mechanicReview.Car);
        }

        if (checkPoint.OperatorReview is not null)
        {
            var operatorReview = checkPoint.OperatorReview;
            var review = new DriverReviewDto(
                operatorReview.Notes,
                operatorReview.Operator.FirstName + " " + operatorReview.Operator.LastName,
                operatorReview.Date,
                ReviewType.OperatorReview,
                operatorReview.Status);
            reviews.Add(review);
        }

        if (checkPoint.MechanicAcceptance is not null)
        {
            var mechanicReview = checkPoint.MechanicAcceptance;
            var review = new DriverReviewDto(
                mechanicReview.Notes,
                mechanicReview.Mechanic.FirstName + " " + mechanicReview.Mechanic.LastName,
                mechanicReview.Date,
                ReviewType.MechanicAcceptance,
                mechanicReview.Status);
            reviews.Add(review);
        }

        var checkPointDto = new DriverCheckPointDto(
            checkPoint.Id,
            checkPoint.StartDate,
            checkPoint.Stage,
            checkPoint.Status,
            checkPoint.DoctorReview.Driver.FirstName + " " + checkPoint.DoctorReview.Driver.LastName,
            car,
            reviews);

        return checkPointDto;
    }
}
