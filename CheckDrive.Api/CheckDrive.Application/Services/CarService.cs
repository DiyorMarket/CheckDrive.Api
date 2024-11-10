using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Entities;
using CheckDrive.Application.QueryParameters;
using CheckDrive.Domain.Enums;
using AutoMapper.QueryableExtensions;

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
            query = query.Where(x => x.Model.Contains(queryParameters.SearchText)
                || x.Color.Contains(queryParameters.SearchText)
                || x.Number.Contains(queryParameters.SearchText));
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
}
