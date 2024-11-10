using AutoMapper;
using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class OilMarkService : IOilMarkService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public OilMarkService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<OilMarkDto> CreateAsync(CreateOilMarkDto oilMark)
    {
        ArgumentNullException.ThrowIfNull(oilMark);

        var entity = _mapper.Map<OilMark>(oilMark);

        _context.OilMarks.Add(entity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OilMarkDto>(entity);

        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.OilMarks.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {id} is not found.");
        }

        _context.OilMarks.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public Task<List<OilMarkDto>> GetAllAsync()
    {
        var oilMarks = _context.OilMarks
            .Select(x => new OilMarkDto(x.Id, x.Name))
            .ToListAsync();

        return oilMarks;
    }

    public async Task<List<OilMarkDto>> GetAllAsync(OilMarkQueryParameters queryParameters)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        var query = _context.OilMarks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(x => x.Name.Contains(queryParameters.SearchText));
        }

        var entities = await query.ToListAsync();
        var dtos = _mapper.Map<List<OilMarkDto>>(entities);

        return dtos;
    }

    public async Task<OilMarkDto> GetByIdAsync(int id)
    {
        var entity = await _context.OilMarks.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {id} is not found.");
        }

        var dto = _mapper.Map<OilMarkDto>(entity);

        return dto;
    }

    public async Task<OilMarkDto> UpdateAsync(UpdateOilMarkDto oilMark)
    {
        if (!await _context.OilMarks.AnyAsync(x => x.Id == oilMark.Id))
        {
            throw new EntityNotFoundException($"Oil mark with id: {oilMark.Id} is not found.");
        }

        var entity = _mapper.Map<OilMark>(oilMark);

        _context.OilMarks.Update(entity);
        await _context.SaveChangesAsync();

        var dto = _mapper.Map<OilMarkDto>(entity);

        return dto;
    }
}
