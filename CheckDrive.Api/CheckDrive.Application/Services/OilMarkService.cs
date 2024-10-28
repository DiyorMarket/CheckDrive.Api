using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class OilMarkService : IOilMarkService
{
    private readonly ICheckDriveDbContext _context;

    public OilMarkService(ICheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task<List<OilMarkDto>> GetAllAsync()
    {
        var oilMarks = _context.OilMarks
            .Select(x => new OilMarkDto(x.Id, x.Name))
            .ToListAsync();

        return oilMarks;
    }
}
