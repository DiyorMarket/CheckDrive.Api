using AutoMapper;
using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Services
{
    public class OilMarkService : IOilMarkService
    {
        private readonly IMapper _mapper;
        private readonly CheckDriveDbContext _context;
        public OilMarkService(IMapper mapper, CheckDriveDbContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<GetBaseResponse<OilMarkDto>> GetMarksAsync(OilMarkResourceParameters resourceParameters)
        {
            var query = GetQueryOilMarkResParameters(resourceParameters);

            var marks = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

            var oilDtos = _mapper.Map<List<OilMarkDto>>(marks);

            var paginatedResult = new PaginatedList<OilMarkDto>(oilDtos, marks.TotalCount, marks.CurrentPage, marks.PageSize);

            return paginatedResult.ToResponse();
        }
        public async Task<OilMarkDto?> GetMarkByIdAsync(int id)
        {
            var oilMark = await _context.OilMarks.FirstOrDefaultAsync(x => x.Id == id);

            var oilMarkDto = _mapper.Map<OilMarkDto>(oilMark);

            return oilMarkDto;
        }
        public async Task<OilMarkDto> CreateMarkAsync(OilMarkForCreateDto markForCreate)
        {
            var oilMarkEntity = _mapper.Map<OilMarks>(markForCreate);

            await _context.OilMarks.AddAsync(oilMarkEntity);
            await _context.SaveChangesAsync();

            var markDto = _mapper.Map<OilMarkDto>(oilMarkEntity);

            return markDto;
        }
        public async Task<OilMarkDto> UpdateMarkAsync(OilMarkForUpdateDto markForUpdate)
        {
            var markEntity = _mapper.Map<OilMarks>(markForUpdate);

            _context.OilMarks.Update(markEntity);
            await _context.SaveChangesAsync();

            var markDto = _mapper.Map<OilMarkDto>(markEntity);

            return markDto;
        }

        public async Task DeleteMarkAsync(int id)
        {
            var oilMark = await _context.OilMarks
                .Include(o => o.OperatorReviews)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (oilMark is not null)
            {
                _context.OperatorReviews.RemoveRange(oilMark.OperatorReviews);
                _context.OilMarks.Remove(oilMark);
            }

            await _context.SaveChangesAsync();
        }
        private IQueryable<OilMarks> GetQueryOilMarkResParameters(
        OilMarkResourceParameters resourceParameters)
        {
            var query = _context.OilMarks.AsQueryable();

            if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            {
                query = resourceParameters.OrderBy.ToLowerInvariant() switch
                {
                    _ => query.OrderBy(x => x.OilMark),
                };
            }

            return query;
        }
    }
}
