using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Domain.QueryParameters;

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

    public async Task<CheckPointDto> GetCurrentCheckPointByDriverIdAsync(int driverId)
    {
        var checkPoint = await GetQuery()
            .Where(x => x.DoctorReview != null)
            .Where(x => x.DoctorReview.DriverId == driverId)
            .Where(x => x.StartDate.Date == DateTime.UtcNow.Date)
            .Where(x => x.Status == CheckPointStatus.InProgress)
            .FirstOrDefaultAsync();

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} does not have current active Check Point.");
        }

        var dto = _mapper.Map<CheckPointDto>(checkPoint);

        return dto;
    }

    private IQueryable<CheckPoint> GetQuery(CheckPointQueryParameters? queryParameters = null)
    {
        var query = _context.CheckPoints
            .AsNoTracking()
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Include(x => x.DoctorReview)
            .ThenInclude(x => x.Doctor)
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
            .AsQueryable();

        if (queryParameters is null)
        {
            return query;
        }

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
}
