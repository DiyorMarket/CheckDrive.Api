using AutoMapper;
using CheckDrive.Api.Extensions;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.DoctorReview;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Hubs;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;

namespace CheckDrive.Services;

public class MechanicHandoverService : IMechanicHandoverService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;
    private readonly IChatHub _chatHub;

    public MechanicHandoverService(IMapper mapper, CheckDriveDbContext context, IChatHub chatHub)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _chatHub = chatHub ?? throw new ArgumentNullException(nameof(chatHub));
    }

    public async Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversAsync(MechanicHandoverResourceParameters resourceParameters)
    {
        var query = GetQueryMechanicHandoverResParameters(resourceParameters);

        if (resourceParameters.Status == Status.Completed || resourceParameters.RoleId == 10)
        {
            var countOfHealthyDrivers = query.Count();
            resourceParameters.MaxPageSize = countOfHealthyDrivers;
            resourceParameters.PageSize = countOfHealthyDrivers;
        }

        var mechanicHandovers = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var mechanicHandoverDtos = _mapper.Map<List<MechanicHandoverDto>>(mechanicHandovers);


        var paginatedResult = new PaginatedList<MechanicHandoverDto>(mechanicHandoverDtos, mechanicHandovers.TotalCount, mechanicHandovers.CurrentPage, mechanicHandovers.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<MechanicHandoverDto?> GetMechanicHandoverByIdAsync(int id)
    {
        var mechanicHandover = await _context.MechanicsHandovers
            .Include(d => d.Car)
            .Include(a => a.Driver)
            .ThenInclude(a => a.Account)
            .Include(m => m.Mechanic)
            .ThenInclude(m => m.Account)
            .FirstOrDefaultAsync(x => x.Id == id);

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandover);

        return mechanicHandoverDto;
    }

    public async Task<MechanicHandoverDto> CreateMechanicHandoverAsync(MechanicHandoverForCreateDto handoverForCreateDto)
    {
        var mechanicHandoverEntity = _mapper.Map<MechanicHandover>(handoverForCreateDto);

        await _context.MechanicsHandovers.AddAsync(mechanicHandoverEntity);

        var car = _context.Cars.FirstOrDefault(x => x.Id == mechanicHandoverEntity.CarId);

        if (car != null)
        {
            car.Mileage = (int)mechanicHandoverEntity.Distance;
            _context.Cars.Update(car);
        }

        await _context.SaveChangesAsync();

        if (mechanicHandoverEntity.IsHanded == true)
        {
            var data = await GetMechanicHandoverByIdAsync(mechanicHandoverEntity.Id);
            var carData = await _context.Cars.FirstOrDefaultAsync(x => x.Id == data.CarId);

            await _chatHub.SendPrivateRequest(new UndeliveredMessageForDto
            {
                SendingMessageStatus = (SendingMessageStatusForDto)SendingMessageStatus.MechanicHandover,
                ReviewId = mechanicHandoverEntity.Id,
                UserId = data.AccountDriverId.ToString(),
                Message = $"Sizga {data.MechanicName} {data.CarName} ni {carData.RemainingFuel} l yoqilg'isi va {carData.Mileage} km bosib o'tilgan masofasi bilan topshirdimi ?"
            });
        }

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverEntity);

        return mechanicHandoverDto;
    }

    public async Task<MechanicHandoverDto> UpdateMechanicHandoverAsync(MechanicHandoverForUpdateDto handoverForUpdateDto)
    {
        var mechanicHandoverEntity = _mapper.Map<MechanicHandover>(handoverForUpdateDto);

        _context.MechanicsHandovers.Update(mechanicHandoverEntity);

        if (mechanicHandoverEntity.Status == Status.Completed)
        {
            var _car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == mechanicHandoverEntity.CarId);

            _car.isBusy = true;
            _car.Mileage = (int)mechanicHandoverEntity.Distance;
            _context.Cars.Update(_car);

            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == mechanicHandoverEntity.DriverId);
            driver.CheckPoint = DriverCheckPoint.PassedMechanicHandover;
            _context.Drivers.Update(driver);
        }
        await _context.SaveChangesAsync();

        var mechanicHandoverDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverEntity);

        return mechanicHandoverDto;
    }

    public async Task DeleteMechanicHandoverAsync(int id)
    {
        var mechanicHandover = await _context.MechanicsHandovers.FirstOrDefaultAsync(x => x.Id == id);

        if (mechanicHandover is not null)
        {
            _context.MechanicsHandovers.Remove(mechanicHandover);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<byte[]> MonthlyExcelData(PropertyForExportFile propertyForExportFile)
    {
        var handovers = _context.MechanicsHandovers
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
            worksheet.Range["A1"].Text = $"Информация о механике(отправка) на эту дату {propertyForExportFile.Month}.{propertyForExportFile.Year}";
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
                worksheet.Range["F" + row].Text = handover.IsHanded ? "topshirildi" : "topshirilmadi";
                worksheet.Range["G" + row].Text = statusMappings[handover.Status];
                worksheet.Range["H" + row].Text = handover.Comments;
            }

            // Save the workbook to a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }

    private IQueryable<MechanicHandover> GetQueryMechanicHandoverResParameters(
        MechanicHandoverResourceParameters resourceParameters)
    {
        var query = _context.MechanicsHandovers
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

        if (resourceParameters.IsHanded is not null)
            query = query.Where(x => x.IsHanded == resourceParameters.IsHanded);

        if (resourceParameters.DriverId is not null)
            query = query.Where(x => x.DriverId == resourceParameters.DriverId);

        var mechanic = _context.Mechanics
            .FirstOrDefault(x => x.AccountId == resourceParameters.AccountId);

        if (resourceParameters.AccountId is not null)
            query = query.Where(x => x.MechanicId == mechanic.Id);

        if (!string.IsNullOrEmpty(resourceParameters.OrderBy))
            query = resourceParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };

        if (resourceParameters.Distance is not null)
            query = query.Where(x => x.Distance == resourceParameters.Distance);
        if (resourceParameters.DistanceLessThan is not null)
            query = query.Where(x => x.Distance < resourceParameters.DistanceLessThan);
        if (resourceParameters.DistanceGreaterThan is not null)
            query = query.Where(x => x.Distance > resourceParameters.DistanceGreaterThan);

        return query;
    }

    public async Task<GetBaseResponse<MechanicHandoverDto>> GetMechanicHandoversForMechanicsAsync(
        MechanicHandoverResourceParameters resourceParameters)
    {
        var doctorReviewsResponse = await _context.DoctorReviews
            .AsNoTracking()
            .Where(x => x.Driver.CheckPoint == DriverCheckPoint.PassedDoctor)
            .Include(x => x.Doctor)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .GroupBy(x => x.DriverId)
            .Select(g => g.OrderByDescending(x => x.Date).FirstOrDefault())
            .ToListAsync();

        var mechanicHandovers = new List<MechanicHandoverDto>();

        foreach (var doctor in doctorReviewsResponse)
        {
            var doctorDto = _mapper.Map<DoctorReviewDto>(doctor);

            mechanicHandovers.Add(new MechanicHandoverDto
            {
                DriverId = doctorDto.DriverId,
                DriverName = doctorDto.DriverName,
                CarName = "",
                MechanicName = "",
                RemainingFuel = 0,
                IsHanded = false,
                Distance = 0,
                Comments = "",
                Date = DateTime.Today.ToTashkentTime().Date,
                Status = ApiContracts.StatusForDto.Unassigned,
            });
        }

        var filteredReviews = ApplyFilters(resourceParameters, mechanicHandovers);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    private List<MechanicHandoverDto> ApplyFilters(MechanicHandoverResourceParameters parameters, List<MechanicHandoverDto> reviews)
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

        if (parameters.Date is not null)
            query = query.Where(x => x.Date.Date == parameters.Date.Value.Date);

        if (parameters.IsHanded is not null)
            query = query.Where(x => x.IsHanded == parameters.IsHanded);

        if (parameters.Status is not null)
            query = query.Where(x => x.Status == (StatusForDto)parameters.Status);

        if (parameters.DriverId is not null)
            query = query.Where(x => x.DriverId == parameters.DriverId);

        if (!string.IsNullOrEmpty(parameters.OrderBy))
            query = parameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };

        if (parameters.Distance is not null)
            query = query.Where(x => x.Distance == parameters.Distance);
        if (parameters.DistanceLessThan is not null)
            query = query.Where(x => x.Distance < parameters.DistanceLessThan);
        if (parameters.DistanceGreaterThan is not null)
            query = query.Where(x => x.Distance > parameters.DistanceGreaterThan);

        return query.ToList();
    }

    private PaginatedList<MechanicHandoverDto> PaginateReviews(List<MechanicHandoverDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<MechanicHandoverDto>(items, totalCount, pageNumber, pageSize);
    }
}

