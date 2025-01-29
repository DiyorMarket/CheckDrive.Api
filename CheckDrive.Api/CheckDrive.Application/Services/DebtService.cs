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
public sealed class DebtService : IDebtService
{
    private readonly ICheckDriveDbContext _context;
    private readonly IMapper _mapper;

    public DebtService(ICheckDriveDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<List<DebtDto>> GetAsync(DebtQueryParametrs queryParameters)
    {
        var query = await GetQuery(queryParameters);        

        var result = await query.AsNoTracking().ProjectTo<DebtDto>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }

    public async Task<DebtDto> GetByIdAsync(int id)
    {
        var debt = await _context.Debts
            .AsNoTracking()
            .Include(d => d.CheckPoint)
            .ThenInclude(cp => cp.DoctorReview)
            .ThenInclude(dr => dr.Driver)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (debt is null)
        {
            throw new EntityNotFoundException($"Debt with id: {id} is not found.");
        }

        var dto = _mapper.Map<DebtDto>(debt);

        return dto;
    }

    public async Task<DebtDto> UpdateAsync(DebtDto debt)
    {
        ArgumentNullException.ThrowIfNull(debt);

        var debts = _mapper.Map<Debt>(debt);

         _context.Debts.Update(debts);

        await _context.SaveChangesAsync();

        return _mapper.Map<DebtDto>(debts);
    }

    public async Task DeleteAsync(int id)
    {
        var debt = _context.Debts.FirstOrDefault(x => x.Id == id);

        if(debt is null)
        {
            throw new EntityNotFoundException($"Debt with id: {id} is not found.");
        }

        _context.Debts.Remove(debt);

        await _context.SaveChangesAsync();
    }
    private async Task<IQueryable<Debt>> GetQuery(DebtQueryParametrs queryParameters)
    {
        var query =  _context.Debts
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
