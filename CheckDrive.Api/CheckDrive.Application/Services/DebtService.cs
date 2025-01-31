using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Exceptions;
using CheckDrive.Domain.Interfaces;
using CheckDrive.Application.QueryParameters;

namespace CheckDrive.Application.Services;
public sealed class DebtService(ICheckDriveDbContext context, IMapper mapper) : IDebtService
{
    public async Task<List<DebtDto>> GetAsync(DebtQueryParametrs queryParameters)
    {
        var query = await GetQuery(queryParameters);        

        var result = await query
            .AsNoTracking()
            .ProjectTo<DebtDto>(mapper.ConfigurationProvider)
            .ToListAsync();

        return result;
    }

    public async Task<DebtDto> GetByIdAsync(int id)
    {
        var debt = await context.Debts
            .AsNoTracking()
            .Include(d => d.CheckPoint)
            .ThenInclude(cp => cp.DoctorReview)
            .ThenInclude(dr => dr.Driver)
            .FirstOrDefaultAsync(x => x.Id == id);        

        return debt is null ? throw new EntityNotFoundException($"Debt with id: {id} is not found.") : mapper.Map<DebtDto>(debt);
    }

    public async Task<DebtDto> UpdateAsync(DebtDto debt)
    {
        ArgumentNullException.ThrowIfNull(debt);

        var resulDebt = mapper.Map<Debt>(debt);

        context.Debts.Update(resulDebt);
        await context.SaveChangesAsync();

        return mapper.Map<DebtDto>(resulDebt);
    }

    public async Task DeleteAsync(int id)
    {
        var debt = context.Debts.FirstOrDefault(x => x.Id == id);

        if(debt is null)
        {
            throw new EntityNotFoundException($"Debt with id: {id} is not found.");
        }

        context.Debts.Remove(debt);
        await context.SaveChangesAsync();
    }
    private async Task<IQueryable<Debt>> GetQuery(DebtQueryParametrs queryParameters)
    {
        var query =  context.Debts
            .Include(d => d.CheckPoint)
            .ThenInclude(cp => cp.DoctorReview)
            .ThenInclude(dr => dr.Driver)
            .AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.SearchText))
        {
            query = query.Where(d =>
                d.CheckPoint.DoctorReview.Driver.FirstName.Contains(queryParameters.SearchText) ||
                d.CheckPoint.DoctorReview.Driver.LastName.Contains(queryParameters.SearchText));
            
        }

        if (queryParameters.Status != null)
        {
            query = query.Where(d => d.Status == queryParameters.Status);
        }

        return query;
    }
}
