using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Interfaces.Review;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services.Review;

internal sealed class DoctorReviewService : IDoctorReviewService
{
    private readonly ICheckDriveDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public DoctorReviewService(
        ICheckDriveDbContext context,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review)
    {
        ArgumentNullException.ThrowIfNull(review);

        var doctor = await GetAndValidateDoctorAsync(review.ReviewerId);
        var driver = await GetAndValidateDriverAsync(review.DriverId);

        var checkPoint = _mapper.Map<CheckPoint>(review);
        checkPoint.Driver = driver;

        var reviewEntity = _mapper.Map<DoctorReview>(review);
        reviewEntity.CheckPoint = checkPoint;
        reviewEntity.Doctor = doctor;

        var createdReview = _context.DoctorReviews.Add(reviewEntity).Entity;

        await _context.SaveChangesAsync();

        var dto = _mapper.Map<DoctorReviewDto>(createdReview);

        return dto;
    }

    private async Task<User> GetAndValidateDoctorAsync(Guid doctorId)
    {
        var doctor = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == doctorId && x.Position == EmployeePosition.Doctor);

        if (doctor is null)
        {
            throw new EntityNotFoundException($"Doctor with id: {doctorId} is not found.");
        }

        var currentUserId = _currentUserService.GetCurrentUserId();

        if (doctor.Id != currentUserId)
        {
            throw new InvalidOperationException($"Only account owner can perform review.");
        }

        return doctor;
    }

    private async Task<User> GetAndValidateDriverAsync(Guid driverId)
    {
        var driver = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == driverId && x.Position == EmployeePosition.Driver);

        if (driver is null)
        {
            throw new EntityNotFoundException($"Driver with id: {driverId} is not found.");
        }

        return driver;
    }
}
