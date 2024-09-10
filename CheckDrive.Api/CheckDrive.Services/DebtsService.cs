using AutoMapper;
using CheckDrive.Api.Extensions;
using CheckDrive.ApiContracts.Debts;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class DebtsService : IDebtsService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;
        public DebtsService(IMapper mapper, CheckDriveDbContext context) 
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetBaseResponse<DebtsDto>> GetDebtsAsync(DebtsResourceParameters resourceParameters)
        {
            var query = GetQueryDebtResParameters(resourceParameters);

            var debts = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var debtDtos = _mapper.Map<List<DebtsDto>>(debts);

            var paginatedResult = new PaginatedList<DebtsDto>(debtDtos, debts.TotalCount, debts.CurrentPage, debts.PageSize);

            return paginatedResult.ToResponse();
        }

        public async Task<DebtsDto?> GetDebtByIdAsync(int id)
        {
            var debt = await _context.Debts
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(d => d.Driver)
                .ThenInclude(d => d.Account)
                .FirstOrDefaultAsync(x => x.Id == id);

            var debtDto = _mapper.Map<DebtsDto>(debt);

            return debtDto;
        }

        public async Task<DebtsDto> CreateDebtAsync(DebtsForCreateDto debtForCreate)
        {
            var debtEntity = _mapper.Map<Debts>(debtForCreate);

            await _context.Debts.AddAsync(debtEntity);
            await _context.SaveChangesAsync();

            var debtDto = _mapper.Map<DebtsDto>(debtEntity);

            return debtDto;
        }

        public async Task<DebtsDto> UpdateDebtAsync(DebtsForUpdateDto debtForUpdate)
        {
            var debtEntity = _mapper.Map<Debts>(debtForUpdate);

            _context.Debts.Update(debtEntity);
            await _context.SaveChangesAsync();

            var debtDto = _mapper.Map<DebtsDto>(debtEntity);

            return debtDto;
        }

        public async Task DeleteDebtAsync(int id)
        {
            var debt = await _context.Debts.FirstOrDefaultAsync(x => x.Id == id);

            if (debt is not null)
            {
                _context.Debts.Remove(debt);
            }

            await _context.SaveChangesAsync();
        }
        
        private IQueryable<Debts> GetQueryDebtResParameters(
           DebtsResourceParameters resourceParameters)
        {
            var query = _context.Debts
                .AsNoTracking()
                .Include(x => x.Car)
                .Include(d => d.Driver)
                .ThenInclude(d => d.Account)
                .AsQueryable();

            if (resourceParameters.Date is not null)
            {
                resourceParameters.Date = DateTime.Today.ToTashkentTime();
                query = query.Where(x => x.Date.Date == resourceParameters.Date.Value.Date);
            }

            if (resourceParameters.OilAmount is not null)
                query = query.Where(x => x.OilAmount == resourceParameters.OilAmount);

            if (resourceParameters.OilAmountLessThan is not null)
                query = query.Where(x => x.OilAmount < resourceParameters.OilAmountLessThan);

            if (resourceParameters.OilAmountGreaterThan is not null)
                query = query.Where(x => x.OilAmount > resourceParameters.OilAmountGreaterThan);

            if (resourceParameters.Status is not null)
                query = query.Where(x => x.Status == resourceParameters.Status);

            if (resourceParameters.DriverId is not null)
                query = query.Where(x => x.DriverId == resourceParameters.DriverId);

            if (resourceParameters.DispatcherReviewId is not null)
                query = query.Where(x => x.DispatcherReviewId == resourceParameters.DispatcherReviewId);

            if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            {
                query = resourceParameters.OrderBy.ToLowerInvariant() switch
                {
                    "oilAmount" => query.OrderBy(x => x.OilAmount),
                    "oilAmountdesc" => query.OrderByDescending(x => x.OilAmount),
                    _ => query.OrderBy(x => x.Id),
                };
            }

            return query;
        }
    }
}
