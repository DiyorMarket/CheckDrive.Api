using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class DriverService : IDriverService
{
    private readonly ICheckDriveDbContext _context;

    public DriverService(ICheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<List<DriverDto>> GetAvailableDriversAsync(CheckPointStage stage)
    {
        var query = _context.Drivers
            .AsNoTracking()
            .Include(x => x.Account)
            .AsQueryable();

        var drivers = await query
            .Select(x => new DriverDto(x.Id, x.AccountId, x.FirstName + " " + x.LastName))
            .ToListAsync();

        return drivers;
    }
}
