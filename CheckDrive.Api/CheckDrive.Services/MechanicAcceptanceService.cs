using AutoMapper;
using CheckDrive.Api.Extensions;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System.Numerics;

namespace CheckDrive.Services;

public class MechanicAcceptanceService : IMechanicAcceptanceService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IChatHub _chatHub;

    public MechanicAcceptanceService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _chatHub = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
    }

    public async Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesAsync(MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicAcceptanceResParameters(resourceParameters);

        query = query.OrderByDescending(item => item.Date);

        if (resourceParameters.Status == Status.Completed || resourceParameters.RoleId == 10)
        {
            var countOfHealthyDrivers = query.Count();
            resourceParameters.MaxPageSize = countOfHealthyDrivers;
            resourceParameters.PageSize = countOfHealthyDrivers;
        }

        var mechanicAcceptances = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicAcceptanceDtos = _mapper.Map<List<MechanicAcceptanceDto>>(mechanicAcceptances);

        var paginatedResult = new PaginatedList<MechanicAcceptanceDto>(mechanicAcceptanceDtos, mechanicAcceptances.TotalCount, mechanicAcceptances.CurrentPage, mechanicAcceptances.PageSize);

        return paginatedResult.ToResponse();

    }

    public async Task<MechanicAcceptanceDto?> GetMechanicAcceptenceByIdAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptance);

        return mechanicAcceptanceDto;
    }

    public async Task<MechanicAcceptanceDto> CreateMechanicAcceptenceAsync(MechanicAcceptanceForCreateDto acceptanceForCreateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForCreateDto);

        await _context.MechanicsAcceptances.AddAsync(mechanicAcceptanceEntity);

        await _context.SaveChangesAsync();

        if (mechanicAcceptanceEntity.IsAccepted == true)
        {
            var data = await GetMechanicAcceptenceByIdAsync(mechanicAcceptanceEntity.Id);

            await _chatHub.SendPrivateRequest(new UndeliveredMessageForDto
            {
                SendingMessageStatus = (SendingMessageStatusForDto)SendingMessageStatus.MechanicAcceptance,
                ReviewId = mechanicAcceptanceEntity.Id,
                UserId = data.AccountDriverId.ToString(),
                Message = $"Siz {data.CarName} avtomobilni {data.MechanicName} ga {data.Distance} km bosib o'tilgan masofasi bilan topshirdizmi ?"
            });
        }

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task<MechanicAcceptanceDto> UpdateMechanicAcceptenceAsync(MechanicAcceptanceForUpdateDto acceptanceForUpdateDto)
    {
        var mechanicAcceptanceEntity = _mapper.Map<MechanicAcceptance>(acceptanceForUpdateDto);

        _context.MechanicsAcceptances.Update(mechanicAcceptanceEntity);

        if (acceptanceForUpdateDto.IsAccepted == true)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == acceptanceForUpdateDto.CarId);
            car.Mileage = (int)acceptanceForUpdateDto.Distance;
            _context.Cars.Update(car);

            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == acceptanceForUpdateDto.DriverId);
            driver.CheckPoint = DriverCheckPoint.PassedMechanicAcceptance;
            _context.Drivers.Update(driver);
        }

        await _context.SaveChangesAsync();

        var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptanceEntity);

        return mechanicAcceptanceDto;
    }

    public async Task DeleteMechanicAcceptenceAsync(int id)
    {
        var mechanicAcceptance = await _context.MechanicsAcceptances.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanicAcceptance is not null)
        {
            _context.MechanicsAcceptances.Remove(mechanicAcceptance);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<byte[]> MonthlyExcelData(PropertyForExportFile propertyForExportFile)
    {
        var handovers = _context.MechanicsAcceptances
            .Where(mh => mh.Date.Month == propertyForExportFile.Month && mh.Date.Year == propertyForExportFile.Year)
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .AsNoTracking()
            .ToList();

        if (handovers.Count == 0) return null;

        var statusMappings = new Dictionary<Status, string>
        {
            { Status.Pending, "Kutilmoqda" },
            { Status.Completed, "Yakunlangan" },
            { Status.Rejected, "Rad etilgan" },
            { Status.Unassigned, "Tayinlanmagan" },
            { Status.RejectedByDriver, "Haydovchi tomonidan rad etilgan" }
        };

        using (ExcelEngine excel = new ExcelEngine())
        {
            IApplication application = excel.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;

            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            // Adding title
            worksheet.Range["A1:K1"].Merge();
            worksheet.Range["A1"].Text = $"Информация о механик(приемник) на эту дату {propertyForExportFile.Month}.{propertyForExportFile.Year}";
            worksheet.Range["A1"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 16;
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            // Adding headers
            worksheet.Range["A2"].Text = "Имя механика";
            worksheet.Range["B2"].Text = "Имя водителя";
            worksheet.Range["C2"].Text = "Название автомобиля";
            worksheet.Range["D2"].Text = "Расстояние";
            worksheet.Range["E2"].Text = "Дата";
            worksheet.Range["F2"].Text = "Вручается";
            worksheet.Range["G2"].Text = "Status";
            worksheet.Range["H2"].Text = "Комментарии";

            // Настройка ширины столбцов
            worksheet.Range["A2:H2"].CellStyle.Font.Bold = true;
            worksheet.Columns[0].ColumnWidth = 20; // MechanicName
            worksheet.Columns[1].ColumnWidth = 20; // DriverName
            worksheet.Columns[2].ColumnWidth = 40; // CarName
            worksheet.Columns[3].ColumnWidth = 15; // Distance
            worksheet.Columns[4].ColumnWidth = 15; // Date
            worksheet.Columns[5].ColumnWidth = 15; // IsHanded
            worksheet.Columns[6].ColumnWidth = 25; // Status
            worksheet.Columns[7].ColumnWidth = 40; // Comments

            // Adding data
            for (int i = 0; i < handovers.Count; i++)
            {
                var handover = handovers[i];
                var row = i + 3;

                worksheet.Range["A" + row].Text = $"{handover.Mechanic.Account.FirstName} {handover.Mechanic.Account.LastName}";
                worksheet.Range["B" + row].Text = $"{handover.Driver.Account.FirstName} {handover.Driver.Account.LastName}";
                worksheet.Range["C" + row].Text = $"{handover.Car.Model} davlat raqami {handover.Car.Number}";
                worksheet.Range["D" + row].Text = handover.Distance.ToString();
                worksheet.Range["E" + row].DateTime = handover.Date;
                worksheet.Range["F" + row].Text = handover.IsAccepted ? "qabul qilindi" : "qabul qilinmadi";
                worksheet.Range["G" + row].Text = statusMappings[handover.Status];
                worksheet.Range["H" + row].Text = handover.Comments;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }

    private IQueryable<MechanicAcceptance> GetQueryMechanicAcceptanceResParameters(
       MechanicAcceptanceResourceParameters resourceParameters)
    {
        var query = _context.MechanicsAcceptances
            .AsNoTracking()
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchString))
            query = query.Where(
                x => x.Driver.Account.FirstName.Contains(resourceParameters.SearchString) ||
                x.Driver.Account.LastName.Contains(resourceParameters.SearchString) ||
                x.Mechanic.Account.FirstName.Contains(resourceParameters.SearchString) ||
                x.Mechanic.Account.LastName.Contains(resourceParameters.SearchString) ||
                x.Comments.Contains(resourceParameters.SearchString));

        if (resourceParameters.Date is not null)
        {
            resourceParameters.Date = DateTime.Today.ToTashkentTime();
            query = query.Where(x => x.Date.Date == resourceParameters.Date.Value.Date);
        }

        if (resourceParameters.Status is not null)
            query = query.Where(x => x.Status == resourceParameters.Status);

        if (resourceParameters.IsAccepted is not null)
            query = query.Where(x => x.IsAccepted == resourceParameters.IsAccepted);

        if (resourceParameters.Distance is not null)
            query = query.Where(x => x.Distance == resourceParameters.Distance);

        if (resourceParameters.DistanceLessThan is not null)
            query = query.Where(x => x.Distance < resourceParameters.DistanceLessThan);

        if (resourceParameters.DistanceGreaterThan is not null)
            query = query.Where(x => x.Distance > resourceParameters.DistanceGreaterThan);

        var mechanic = _context.Mechanics
            .FirstOrDefault(x => x.AccountId == resourceParameters.AccountId);

        if (resourceParameters.DriverId is not null)
            query = query.Where(x => x.DriverId == resourceParameters.DriverId);

        if (resourceParameters.AccountId is not null)
            query = query.Where(x => x.MechanicId == mechanic.Id);

        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
        {
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }

        return query;
    }

    public async Task<GetBaseResponse<MechanicAcceptanceDto>> GetMechanicAcceptencesForMechanicAsync(
    MechanicAcceptanceResourceParameters resourceParameters)
    {
        // Fetch the latest operator review for each driver
        var operatorReviewsResponse = await _context.OperatorReviews
            .AsNoTracking()
            .Where(dr => dr.Driver.CheckPoint == DriverCheckPoint.PassedOperator)
            .Include(x => x.Operator)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.OilMark)
            .GroupBy(x => x.DriverId)
            .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
            .ToListAsync();

        // Fetch all mechanic handovers
        var mechanicAcceptanseContext = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Where(x => x.Status == Status.Pending)
            .ToListAsync();

        // Prepare the list of mechanic acceptances to be returned
        var mechanicAcceptance = new List<MechanicAcceptanceDto>();

        foreach (var operatorReview in operatorReviewsResponse)
        {
            var operatorReviewDto = _mapper.Map<OperatorReviewDto>(operatorReview);
            var mechanicAcceptanceResult = mechanicAcceptanseContext
                .FirstOrDefault(mh => mh.DriverId == operatorReview.DriverId);

            // Safeguard for null operatorReviewDto
            string carName = $"{operatorReviewDto?.CarModel ?? ""} ({operatorReviewDto?.CarNumber ?? ""})";
            string driverName = operatorReviewDto?.DriverName ?? "";
            double remainingFuel = operatorReview?.Car?.RemainingFuel ?? 0;
            DateTime date = mechanicAcceptanceResult?.Date ?? DateTime.Today.ToTashkentTime().Date;
            var status = mechanicAcceptanceResult != null ? ApiContracts.StatusForDto.Pending : ApiContracts.StatusForDto.Unassigned;

            // Safeguard for null mechanicAcceptanceResult and its nested properties
            var mechanicName = mechanicAcceptanceResult?.Mechanic?.Account?.FirstName ?? "";
            var isAccepted = mechanicAcceptanceResult?.IsAccepted ?? false;
            var distance = mechanicAcceptanceResult?.Distance ?? 0;
            var comments = mechanicAcceptanceResult?.Comments ?? "";
            var oilAmount = mechanicAcceptanceResult?.OilAmount ?? 0;

            mechanicAcceptance.Add(new MechanicAcceptanceDto
            {
                DriverId = operatorReviewDto?.DriverId ?? 0,
                DriverName = driverName,
                CarId = operatorReviewDto?.CarId ?? 0,
                CarName = carName,
                MechanicName = mechanicName,
                RemainingFuel = remainingFuel,
                OilAmount = oilAmount,
                IsAccepted = isAccepted,
                Distance = distance,
                Comments = comments,
                Date = date,
                Status = status,
            });
        }

    // Apply filters and pagination
    var filteredReviews = ApplyFilters(resourceParameters, mechanicAcceptance);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }



    private List<MechanicAcceptanceDto> ApplyFilters(MechanicAcceptanceResourceParameters parameters, List<MechanicAcceptanceDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchString))
        {
            var searchString = parameters.SearchString.ToLowerInvariant();
            query = query.Where(x =>
                (!string.IsNullOrEmpty(x.DriverName) && x.DriverName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.MechanicName) && x.MechanicName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.CarName) && x.CarName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.Comments) && x.Comments.ToLowerInvariant().Contains(searchString)));
        }

        if (parameters.Date != null)
            query = query.Where(x => x.Date.Value.Date == parameters.Date.Value.Date);

        if (parameters.DriverId != null)
            query = query.Where(x => x.DriverId == parameters.DriverId);

        if (parameters.Status is not null)
            query = query.Where(x => x.Status == (StatusForDto)parameters.Status);

        if (!string.IsNullOrEmpty(parameters.OrderBy))
            query = parameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.DriverId),
            };

        return query.ToList();
    }

    private PaginatedList<MechanicAcceptanceDto> PaginateReviews(List<MechanicAcceptanceDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<MechanicAcceptanceDto>(items, totalCount, pageNumber, pageSize);
    }
}

