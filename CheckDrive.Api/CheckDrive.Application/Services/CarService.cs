using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using AutoMapper.QueryableExtensions;
using CheckDrive.Application.DTOs.Ride;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Services;

internal sealed class CarService : ICarService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public CarService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<CarDto>> GetAvailableCarsAsync()
    {
        var cars = await _context.Cars
            .Where(x => x.Status == CarStatus.Free)
            .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return cars;
    }

    public async Task<List<CarDto>> GetAllAsync(CarQueryParameters queryParameters)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        var query = _context.Cars
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(x => x.Model.Contains(queryParameters.SearchText) || x.Number.Contains(queryParameters.SearchText));
        }

        if (queryParameters.Status.HasValue)
        {
            query = query.Where(x => x.Status == queryParameters.Status);
        }

        var cars = await query
            .ProjectTo<CarDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return cars;
    }

    public async Task<CarDto> GetByIdAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {id} is not found.");
        }

        var dto = _mapper.Map<CarDto>(car);

        return dto;
    }

    public async Task<CarDto> CreateAsync(CreateCarDto car)
    {
        ArgumentNullException.ThrowIfNull(car);

        var entity = _mapper.Map<Car>(car);

        _context.Cars.Add(entity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<CarDto>(entity);

        return dto;
    }

    public async Task<CarDto> UpdateAsync(UpdateCarDto car)
    {
        ArgumentNullException.ThrowIfNull(car);

        if (!await _context.Cars.AnyAsync(x => x.Id == car.Id))
        {
            throw new EntityNotFoundException($"Car with id: {car.Id} is not found.");
        }

        var entity = _mapper.Map<Car>(car);

        _context.Cars.Update(entity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<CarDto>(entity);

        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Car with id: {id} is not found.");
        }

        _context.Cars.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task CompleteRide(RideDetailsDto rideDetails)
    {
        ArgumentNullException.ThrowIfNull(rideDetails);

        var car = await GetAndValidateCarAsync(rideDetails);

        var travelledDistance = rideDetails.FinalMileage - car.Mileage;

        if (car.RemainingFuel < rideDetails.FuelConsumptionAmount)
        {
            var debtAmount = car.RemainingFuel + rideDetails.RemainingFuelAmount - rideDetails.FuelConsumptionAmount;
            var checkPoint = await GetAndValidateCheckPointAsync(rideDetails.CheckPointId);
            var debt = new Debt
            {
                FuelAmount = debtAmount,
                CheckPoint = checkPoint,
                Status = DebtStatus.Unpaid,
            };

            _context.Debts.Add(debt);
        }

        car.RemainingFuel = rideDetails.RemainingFuelAmount;

        if (IsCarExceededLimits(car))
        {
            car.Status = CarStatus.LimitReached;
        }

        await _context.SaveChangesAsync();
    }

    private async Task<Car> GetAndValidateCarAsync(RideDetailsDto rideDetails)
    {
        var car = await _context.Cars
            .FirstOrDefaultAsync(x => x.Id == rideDetails.CarId);

        if (car is null)
        {
            throw new EntityNotFoundException($"Car with id: {rideDetails.CarId} is not found.");
        }

        if (car.Mileage > rideDetails.FinalMileage)
        {
            throw new InvalidOperationException($"Final mileage ({rideDetails.FinalMileage}) cannot be less than current mileage ({car.Mileage}).");
        }

        return car;
    }

    private async Task<CheckPoint> GetAndValidateCheckPointAsync(int checkPointId)
    {
        var checkPoint = await _context.CheckPoints
            .FirstOrDefaultAsync(x => x.Id == checkPointId);

        if (checkPoint is null)
        {
            throw new EntityNotFoundException($"Check Point with id: {checkPointId} is not found.");
        }

        return checkPoint;
    }

    private static bool IsCarExceededLimits(Car car)
        => IsCarExceededMonthlyLimit(car) || IsCarExceededYearlyLimit(car);

    private static bool IsCarExceededMonthlyLimit(Car car)
    {

        return false;
    }

    private static bool IsCarExceededYearlyLimit(Car car)
    {

        return false;
    }
}
