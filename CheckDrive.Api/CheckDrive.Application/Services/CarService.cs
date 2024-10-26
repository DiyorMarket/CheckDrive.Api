using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            .ToListAsync();

        var dtos = _mapper.Map<List<CarDto>>(cars);

        return dtos;
    }
}
