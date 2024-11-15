﻿using AutoMapper;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class ReviewHistoryService : IReviewHistoryService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public ReviewHistoryService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<CheckPointDto>> GetDriverHistoriesAsync(int driverId)
    {
        var checkPoints = await _context.CheckPoints
            .AsNoTracking()
            .Where(x => x.DoctorReview.DriverId == driverId)
            .Where(x => x.Status == CheckPointStatus.Completed || x.Status == CheckPointStatus.AutomaticallyClosed)
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
            .Include(x => x.OperatorReview)
            .ThenInclude(x => x.OilMark)
            .Include(x => x.MechanicAcceptance)
            .ThenInclude(x => x.Mechanic)
            .Include(x => x.DispatcherReview)
            .Include(x => x.ManagerReview)
            .Include(x => x.Debt)
            .AsSplitQuery()
            .ToListAsync();

        var dtos = _mapper.Map<List<CheckPointDto>>(checkPoints);

        return dtos;

    }

    public async Task<List<DoctorReviewDto>> GetDoctorHistoriesAsync(int doctorId)
    {
        var reviews = await _context.DoctorReviews
            .Include(x => x.Driver)
            .Where(x => x.DoctorId == doctorId)
            .ToListAsync();

        var dtos = _mapper.Map<List<DoctorReviewDto>>(reviews);

        return dtos;
    }

    public async Task<List<MechanicReviewHistoryDto>> GetMechanicHistoriesAsync(int mechanicId)
    {
        var handovers = await _context.MechanicHandovers
            .AsNoTracking()
            .Where(x => x.MechanicId == mechanicId)
            .Include(x => x.CheckPoint)
            .ThenInclude(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Select(x => new MechanicReviewHistoryDto(
                x.Id,
                x.CheckPoint.DoctorReview.DriverId,
                x.CheckPoint.DoctorReview.Driver.FirstName + " " + x.CheckPoint.DoctorReview.Driver.LastName,
                x.Notes,
                x.Date,
                x.Status))
            .ToListAsync();
        var acceptances = await _context.MechanicAcceptances
            .Include(x => x.CheckPoint)
            .ThenInclude(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .Select(x => new MechanicReviewHistoryDto(
                x.Id,
                x.CheckPoint.DoctorReview.DriverId,
                x.CheckPoint.DoctorReview.Driver.FirstName + " " + x.CheckPoint.DoctorReview.Driver.LastName,
                x.Notes,
                x.Date,
                x.Status))
            .ToListAsync();

        List<MechanicReviewHistoryDto> allReviews = [.. handovers, .. acceptances];
        var orderedReviews = allReviews.OrderByDescending(x => x.Date).ToList();

        return orderedReviews;
    }

    public async Task<List<OperatorReviewDto>> GetOperatorHistoriesAsync(int operatorId)
    {
        var reviews = await _context.OperatorReviews
            .AsNoTracking()
            .Where(x => x.OperatorId == operatorId)
            .Include(x => x.Operator)
            .Include(x => x.OilMark)
            .Include(x => x.CheckPoint)
            .ThenInclude(x => x.DoctorReview)
            .ThenInclude(x => x.Driver)
            .AsSplitQuery()
            .Select(x => new OperatorReviewDto(
                x.CheckPointId,
                x.OperatorId,
                x.Operator.FirstName + " " + x.Operator.LastName,
                x.CheckPoint.DoctorReview.DriverId,
                x.CheckPoint.DoctorReview.Driver.FirstName + " " + x.CheckPoint.DoctorReview.Driver.LastName,
                x.OilMarkId,
                x.OilMark.Name,
                x.Notes,
                x.Date,
                x.Status,
                x.InitialOilAmount,
                x.OilRefillAmount))
            .ToListAsync();

        return reviews;
    }
}