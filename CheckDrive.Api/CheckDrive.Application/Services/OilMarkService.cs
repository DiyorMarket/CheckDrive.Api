using AutoMapper;
using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Application.Interfaces;
using CheckDrive.Application.QueryParameters;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class OilMarkService(ICheckDriveDbContext context, IMapper mapper) : IOilMarkService
{
    public async Task<List<OilMarkDto>> GetAllAsync(OilMarkQueryParameters queryParameters)
    {
        ArgumentNullException.ThrowIfNull(queryParameters);

        var query = context.OilMarks.AsNoTracking().AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(x => x.Name.Contains(queryParameters.SearchText));
        }

        var entities = await query.ToListAsync();
        var dtos = mapper.Map<List<OilMarkDto>>(entities);

        return dtos;
    }

    public async Task<OilMarkDto> GetByIdAsync(int id)
    {
        var entity = await context.OilMarks.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {id} is not found.");
        }

        var dto = mapper.Map<OilMarkDto>(entity);

        return dto;
    }

    public async Task<OilMarkDto> UpdateAsync(UpdateOilMarkDto oilMark)
    {
        if (!await context.OilMarks.AnyAsync(x => x.Id == oilMark.Id))
        {
            throw new EntityNotFoundException($"Oil mark with id: {oilMark.Id} is not found.");
        }

        var entity = mapper.Map<OilMark>(oilMark);

        context.OilMarks.Update(entity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<OilMarkDto>(entity);

        return dto;
    }

    public async Task<OilMarkDto> CreateAsync(CreateOilMarkDto oilMark)
    {
        ArgumentNullException.ThrowIfNull(oilMark);

        var entity = mapper.Map<OilMark>(oilMark);

        context.OilMarks.Add(entity);
        await context.SaveChangesAsync();

        var dto = mapper.Map<OilMarkDto>(entity);

        return dto;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.OilMarks.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"Oil mark with id: {id} is not found.");
        }

        context.OilMarks.Remove(entity);
        await context.SaveChangesAsync();
    }
}
