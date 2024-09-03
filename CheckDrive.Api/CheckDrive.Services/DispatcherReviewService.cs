using AutoMapper;
using CheckDrive.Api.Extensions;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.ApiContracts.MechanicAcceptance;
using CheckDrive.ApiContracts.MechanicHandover;
using CheckDrive.ApiContracts.OperatorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation.Security;
using System.Data.SqlTypes;

namespace CheckDrive.Services;

public class DispatcherReviewService : IDispatcherReviewService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public DispatcherReviewService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsAsync(DispatcherReviewResourceParameters resourceParameters)
    {
        var query = GetQueryDispatcherReviewResParameters(resourceParameters);

        if (resourceParameters.RoleId == 10)
        {
            var countOfHealthyDrivers = query.Count();
            resourceParameters.MaxPageSize = countOfHealthyDrivers;
            resourceParameters.PageSize = countOfHealthyDrivers;
        }

        var dispatcherReviews = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var dispatcherReviewsDto = _mapper.Map<List<DispatcherReviewDto>>(dispatcherReviews);

        var paginatedResult = new PaginatedList<DispatcherReviewDto>(dispatcherReviewsDto, dispatcherReviews.TotalCount, dispatcherReviews.CurrentPage, dispatcherReviews.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<DispatcherReviewDto?> GetDispatcherReviewByIdAsync(int id)
    {
        var dispatcherReview = await _context.DispatchersReviews
            .AsNoTracking()
            .Include(x => x.Car)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.OperatorReview)
            .Include(d => d.MechanicAcceptance)
            .Include(d => d.MechanicHandover)
            .FirstOrDefaultAsync(x => x.Id == id);

        var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherReview);

        return dispatcherReviewDto;
    }

    public async Task<DispatcherReviewDto> CreateDispatcherReviewAsync(DispatcherReviewForCreateDto dispatcherReviewForCreate)
    {
        try
        {
            var dispatcherEntity = _mapper.Map<DispatcherReview>(dispatcherReviewForCreate);

            bool isDistanceChanged = dispatcherEntity.ChangedDistanceCovered.HasValue &&
                                     dispatcherEntity.DistanceCovered != dispatcherEntity.ChangedDistanceCovered;

            bool isFuelChanged = dispatcherEntity.ChangedFuelSpendede.HasValue &&
                                 dispatcherEntity.FuelSpended != dispatcherEntity.ChangedFuelSpendede;

            if (isDistanceChanged || isFuelChanged)
            {
                dispatcherEntity.Status = Status.Rejected;
            }
            else
            {
                dispatcherEntity.Status = Status.Completed;
            }

            dispatcherEntity.ChangedDistanceCovered ??= 0;
            dispatcherEntity.ChangedFuelSpendede ??= 0;

            var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dispatcherReviewForCreate.CarId);
            var mechanicHandover = await _context.MechanicsHandovers.FirstOrDefaultAsync(x => x.Id == dispatcherReviewForCreate.MechanicHandoverId);
            var mechanicAcceptence = await _context.MechanicsAcceptances.FirstOrDefaultAsync(x => x.Id == dispatcherReviewForCreate.MechanicAcceptanceId);
            if (car == null || mechanicHandover == null || mechanicAcceptence == null)
            {
                throw new InvalidOperationException("Related entities not found. Please check the provided IDs.");
            }
            DateTime now = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            var firstDispatcherReview = await _context.DispatchersReviews
                .AsNoTracking()
                .Where(review => review.Date >= firstDayOfMonth
                     && review.CarId == car.Id)
                .OrderBy(review => review.Date)
                .FirstOrDefaultAsync();

            if (firstDispatcherReview != null)
            {
                var monthlyDistance = car.OneYearMediumDistance / 12;
                if (monthlyDistance > 0)
                {
                    var distancee = (int)mechanicHandover.Distance;

                    var total = car.Mileage - distancee;

                    if (monthlyDistance < total)
                    {
                        car.CarStatus = CarStatus.Limited;
                        _context.Cars.Update(car);
                    }
                }
            }
            else
            {
                car.CarStatus = CarStatus.Free;
                _context.Cars.Update(car);
            }

            var initialDistance = mechanicHandover.Distance;
            var finishDistance = mechanicAcceptence.Distance;

            var distence = finishDistance - initialDistance;

            if (distence < dispatcherEntity.DistanceCovered)
            {
                var totalDistence = dispatcherEntity.DistanceCovered - distence;
                car.Mileage += (int)totalDistence;

            }
            else if (distence > dispatcherEntity.DistanceCovered)
            {
                var totalDistence = distence - dispatcherEntity.DistanceCovered;
                car.Mileage -= (int)totalDistence;
            }

            car.RemainingFuel -= dispatcherEntity.FuelSpended;
            _context.Cars.Update(car);

            var driver = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == dispatcherReviewForCreate.DriverId);
            driver.CheckPoint = DriverCheckPoint.Initial;
            _context.Drivers.Update(driver);

            await _context.DispatchersReviews.AddAsync(dispatcherEntity);
            await _context.SaveChangesAsync();

            var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherEntity);

            return dispatcherReviewDto;
        }
        catch (SqlNullValueException ex)
        {
            // Логируйте или обрабатывайте ошибку
            Console.WriteLine($"Ошибка при получении данных: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<DispatcherReviewDto> UpdateDispatcherReviewAsync(DispatcherReviewForUpdateDto dispatcherReviewForUpdate)
    {
        var dispatcherEntity = _mapper.Map<DispatcherReview>(dispatcherReviewForUpdate);

        _context.DispatchersReviews.Update(dispatcherEntity);
        await _context.SaveChangesAsync();

        var existingReview = await _context.DispatchersReviews
            .FirstOrDefaultAsync(x => x.Id == dispatcherEntity.Id);

        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == dispatcherEntity.CarId);

        car.RemainingFuel = car.RemainingFuel + (double)existingReview.FuelSpended - dispatcherReviewForUpdate.FuelSpended;
        _context.Cars.Update(car);


        var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherEntity);

        return dispatcherReviewDto;
    }

    public async Task DeleteDispatcherReviewAsync(int id)
    {
        var dispatcherReview = await _context.DispatchersReviews
            .FirstOrDefaultAsync(x => x.Id == id);

        if (dispatcherReview is not null)
        {
            _context.DispatchersReviews.Remove(dispatcherReview);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<byte[]> MonthlyExcelData(PropertyForExportFile propertyForExportFile)
    {
        var handovers = _context.DispatchersReviews
            .Where(mh => mh.Date.Month == propertyForExportFile.Month && mh.Date.Year == propertyForExportFile.Year)
            .Include(ma => ma.MechanicAcceptance)
            .ThenInclude(ma => ma.Mechanic)
            .ThenInclude(ma => ma.Account)
            .Include(mh => mh.MechanicHandover)
            .ThenInclude(mh => mh.Mechanic)
            .ThenInclude(mh => mh.Account)
            .Include(o => o.OperatorReview)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.Car)
            .AsNoTracking()
            .ToList();

        if (handovers.Count == 0) return null;

        using (ExcelEngine excel = new ExcelEngine())
        {
            IApplication application = excel.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;

            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            // Adding title
            worksheet.Range["A1:K1"].Merge();
            worksheet.Range["A1"].Text = $"Информация об Диспетчерские услуги на эту дату {propertyForExportFile.Month}.{propertyForExportFile.Year}";
            worksheet.Range["A1"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 16;
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            // Adding headers
            worksheet.Range["A2"].Text = "Имя диспетчера";
            worksheet.Range["B2"].Text = "Название автомобиля";
            worksheet.Range["C2"].Text = "Имя механика сдачи";
            worksheet.Range["D2"].Text = "Имя оператора осмотра";
            worksheet.Range["E2"].Text = "Имя механика приемки";
            worksheet.Range["F2"].Text = "Дата";
            worksheet.Range["G2"].Text = "Расход топлива";
            worksheet.Range["H2"].Text = "Пройденное расстояние";

            // Настройка ширины столбцов
            worksheet.Range["A2:H2"].CellStyle.Font.Bold = true;
            worksheet.Columns[0].ColumnWidth = 20; // Имя диспетчера
            worksheet.Columns[1].ColumnWidth = 40; // Название автомобиля
            worksheet.Columns[2].ColumnWidth = 20; // Имя механика сдачи
            worksheet.Columns[3].ColumnWidth = 20; // Имя оператора осмотра
            worksheet.Columns[4].ColumnWidth = 20; // Имя механика приемки
            worksheet.Columns[5].ColumnWidth = 15; // Дата
            worksheet.Columns[6].ColumnWidth = 15; // Израсходовано топлива
            worksheet.Columns[7].ColumnWidth = 15; // Пробег

            // Adding data
            for (int i = 0; i < handovers.Count; i++)
            {
                var handover = handovers[i];
                var row = i + 3;

                worksheet.Range["A" + row].Text = $"{handover.Dispatcher.Account.FirstName} {handover.Dispatcher.Account.LastName}";
                worksheet.Range["B" + row].Text = $"{handover.Car.Model} davlat raqami {handover.Car.Number}";
                worksheet.Range["C" + row].Text = $"{handover.MechanicHandover.Mechanic.Account.FirstName} {handover.MechanicHandover.Mechanic.Account.LastName}";
                worksheet.Range["D" + row].Text = $"{handover.Operator.Account.FirstName} {handover.Operator.Account.LastName}";
                worksheet.Range["E" + row].Text = $"{handover.MechanicAcceptance.Mechanic.Account.FirstName} {handover.MechanicAcceptance.Mechanic.Account.LastName}";
                worksheet.Range["F" + row].DateTime = handover.Date;
                worksheet.Range["G" + row].Number = handover.FuelSpended;
                worksheet.Range["H" + row].Number = handover.DistanceCovered;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }

    private IQueryable<DispatcherReview> GetQueryDispatcherReviewResParameters(
       DispatcherReviewResourceParameters dispatcherReviewParameters)
    {
        var query = _context.DispatchersReviews
            .AsNoTracking()
            .Include(ma => ma.MechanicAcceptance)
            .Include(mh => mh.MechanicHandover)
            .Include(o => o.OperatorReview)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.Car)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(dispatcherReviewParameters.SearchString))
            query = query.Where(
                x => x.Driver.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Driver.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Mechanic.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Mechanic.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Operator.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Operator.Account.LastName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Dispatcher.Account.FirstName.Contains(dispatcherReviewParameters.SearchString) ||
                x.Dispatcher.Account.LastName.Contains(dispatcherReviewParameters.SearchString));

        if (dispatcherReviewParameters.Date is not null)
        {
            dispatcherReviewParameters.Date = DateTime.Today.ToTashkentTime();
            query = query.Where(x => x.Date.Date == dispatcherReviewParameters.Date.Value.Date);
        }

        //FuelSpended
        var dispatcher = _context.Dispatchers
            .FirstOrDefault(x => x.AccountId == dispatcherReviewParameters.AccountId);
        if (dispatcherReviewParameters.AccountId is not null)
        {
            query = query.Where(x => x.DispatcherId == dispatcher.Id);
        }
        if (dispatcherReviewParameters.DriverId is not null)
        {
            query = query.Where(x => x.DriverId == dispatcherReviewParameters.DriverId);
        }
        if (dispatcherReviewParameters.FuelSpended is not null)
        {
            query = query.Where(x => x.FuelSpended == dispatcherReviewParameters.FuelSpended);
        }
        if (dispatcherReviewParameters.FuelSpendedLessThan is not null)
        {
            query = query.Where(x => x.FuelSpended < dispatcherReviewParameters.FuelSpendedLessThan);
        }
        if (dispatcherReviewParameters.FuelSpendedGreaterThan is not null)
        {
            query = query.Where(x => x.FuelSpended > dispatcherReviewParameters.FuelSpendedGreaterThan);
        }

        //DistanceCovered
        if (dispatcherReviewParameters.DistanceCovered is not null)
        {
            query = query.Where(x => x.DistanceCovered == dispatcherReviewParameters.DistanceCovered);
        }
        if (dispatcherReviewParameters.DistanceCoveredLessThan is not null)
        {
            query = query.Where(x => x.DistanceCovered < dispatcherReviewParameters.DistanceCoveredLessThan);
        }
        if (dispatcherReviewParameters.DistanceCoveredGreaterThan is not null)
        {
            query = query.Where(x => x.DistanceCovered > dispatcherReviewParameters.DistanceCoveredGreaterThan);
        }

        if (!string.IsNullOrEmpty(dispatcherReviewParameters.OrderBy))
        {
            query = dispatcherReviewParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.Id),
            };
        }
        return query;
    }

    public async Task<GetBaseResponse<DispatcherReviewDto>> GetDispatcherReviewsForDispatcherAsync(DispatcherReviewResourceParameters resourceParameters)
    {
        var mechanicAcceptanceResponse = await _context.MechanicsAcceptances
            .AsNoTracking()
            .Where(x => x.Driver.CheckPoint == DriverCheckPoint.PassedMechanicAcceptance)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .GroupBy(x => x.DriverId)
            .Select(x => x.OrderByDescending(x => x.Date).FirstOrDefault())
            .ToListAsync();

        var mechanicHandoverResponse = await _context.MechanicsHandovers
            .AsNoTracking()
            .Where(x => x.Status == Status.Completed)
            .OrderByDescending(x => x.Date)
            .Include(x => x.Mechanic)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .ToListAsync();

        var operatorResponse = await _context.OperatorReviews
            .AsNoTracking()
            .Where(x => x.Status == Status.Completed)
            .OrderByDescending(x => x.Date)
            .Include(x => x.Operator)
            .ThenInclude(x => x.Account)
            .Include(x => x.Driver)
            .ThenInclude(x => x.Account)
            .Include(x => x.Car)
            .Include(x => x.OilMark)
            .ToListAsync();

        var carResponse = await _context.Cars
            .ToListAsync();

        var dispatchers = new List<DispatcherReviewDto>();

        foreach (var mechanicAcceptance in mechanicAcceptanceResponse)
        {
            var mechanicHandoverReview = mechanicHandoverResponse.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId);
            var operatorReview = operatorResponse.FirstOrDefault(m => m.DriverId == mechanicAcceptance.DriverId);
            var carReview = carResponse.FirstOrDefault(c => c.Id == mechanicAcceptance.CarId);

            var mechanicHandoverReviewDto = _mapper.Map<MechanicHandoverDto>(mechanicHandoverReview);
            var mechanicAcceptanceDto = _mapper.Map<MechanicAcceptanceDto>(mechanicAcceptance);

            var operatorReviewDto = _mapper.Map<OperatorReviewDto>(operatorReview);
            var carReviewDto = _mapper.Map<CarDto>(carReview);


            dispatchers.Add(new DispatcherReviewDto
            {
                DriverId = mechanicAcceptanceDto.DriverId,
                DriverName = mechanicAcceptanceDto.DriverName,
                CarId = mechanicAcceptanceDto.CarId,
                CarName = mechanicAcceptanceDto.CarName,
                CarMeduimFuelConsumption = mechanicAcceptance.Car.MeduimFuelConsumption,

                FuelSpended = (mechanicAcceptanceDto.Distance - mechanicHandoverReviewDto.Distance) * carReviewDto.MeduimFuelConsumption / 100,
                DistanceCovered = mechanicAcceptanceDto.Distance - mechanicHandoverReviewDto.Distance,

                InitialDistance = mechanicHandoverReviewDto.Distance,
                FinalDistance = mechanicAcceptanceDto.Distance,
                PouredFuel = operatorReviewDto.OilAmount ?? 0,

                RemainigFuelBefore = carReviewDto.RemainingFuel - (double)operatorReviewDto.OilAmount,
                RemainigFuelAfter = carReviewDto.RemainingFuel - ((mechanicAcceptanceDto.Distance - mechanicHandoverReviewDto.Distance) * carReviewDto.MeduimFuelConsumption / 100),
                OperatorName = operatorReviewDto.OperatorName,
                OperatorReviewId = operatorReviewDto.Id,
                DispatcherName = "",
                MechanicName = mechanicAcceptanceDto.MechanicName,

                Date = DateTime.Today.ToTashkentTime().Date,
                MechanicAcceptanceId = mechanicAcceptanceDto.Id,
                MechanicHandoverId = mechanicHandoverReviewDto.Id,
                OperatorId = operatorReviewDto.OperatorId,
                MechanicId = mechanicAcceptanceDto.MechanicId
            });
        }

        var filteredReviews = ApplyFilters(resourceParameters, dispatchers);
        var paginatedResult = PaginateReviews(filteredReviews, resourceParameters.PageSize, resourceParameters.PageNumber);

        return paginatedResult.ToResponse();
    }

    private List<DispatcherReviewDto> ApplyFilters(DispatcherReviewResourceParameters dispatcherReviewParameters, List<DispatcherReviewDto> reviews)
    {
        var query = reviews.AsQueryable();

        if (!string.IsNullOrWhiteSpace(dispatcherReviewParameters.SearchString))
        {
            var searchString = dispatcherReviewParameters.SearchString.ToLowerInvariant();
            query = query.Where(x =>
                (!string.IsNullOrEmpty(x.DriverName) && x.DriverName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.MechanicName) && x.MechanicName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.OperatorName) && x.OperatorName.ToLowerInvariant().Contains(searchString)) ||
                (!string.IsNullOrEmpty(x.DispatcherName) && x.DispatcherName.ToLowerInvariant().Contains(searchString) ||
                (!string.IsNullOrEmpty(x.CarName) && x.CarName.ToLowerInvariant().Contains(searchString))));
        }

        if (dispatcherReviewParameters.Date is not null)
            query = query.Where(x => x.Date.Date == dispatcherReviewParameters.Date.Value.Date);

        if (dispatcherReviewParameters.DriverId is not null)
            query = query.Where(x => x.DriverId == dispatcherReviewParameters.DriverId);
        if (dispatcherReviewParameters.FuelSpended is not null)
            query = query.Where(x => x.FuelSpended == dispatcherReviewParameters.FuelSpended);
        if (dispatcherReviewParameters.FuelSpendedLessThan is not null)
            query = query.Where(x => x.FuelSpended < dispatcherReviewParameters.FuelSpendedLessThan);
        if (dispatcherReviewParameters.FuelSpendedGreaterThan is not null)
            query = query.Where(x => x.FuelSpended > dispatcherReviewParameters.FuelSpendedGreaterThan);

        if (dispatcherReviewParameters.DistanceCovered is not null)
            query = query.Where(x => x.DistanceCovered == dispatcherReviewParameters.DistanceCovered);
        if (dispatcherReviewParameters.DistanceCoveredLessThan is not null)
            query = query.Where(x => x.DistanceCovered < dispatcherReviewParameters.DistanceCoveredLessThan);
        if (dispatcherReviewParameters.DistanceCoveredGreaterThan is not null)
            query = query.Where(x => x.DistanceCovered > dispatcherReviewParameters.DistanceCoveredGreaterThan);

        if (!string.IsNullOrEmpty(dispatcherReviewParameters.OrderBy))
        {
            query = dispatcherReviewParameters.OrderBy.ToLowerInvariant() switch
            {
                "date" => query.OrderBy(x => x.Date),
                "datedesc" => query.OrderByDescending(x => x.Date),
                _ => query.OrderBy(x => x.DriverId),
            };
        }

        return query.ToList();
    }

    private PaginatedList<DispatcherReviewDto> PaginateReviews(List<DispatcherReviewDto> reviews, int pageSize, int pageNumber)
    {
        var totalCount = reviews.Count;
        var items = reviews.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<DispatcherReviewDto>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<IEnumerable<DispatcherReviewDto>> GetDispatcherHistories(int? Id)
    {
        var dispatcher = await _context.Dispatchers
                .Where(x => x.AccountId == Id)
                .FirstOrDefaultAsync();

        var dispatcherHistories = _context.DispatchersReviews
            .AsNoTracking()
            .Include(ma => ma.MechanicAcceptance)
            .Include(mh => mh.MechanicHandover)
            .Include(o => o.OperatorReview)
            .Include(d => d.Driver)
            .ThenInclude(d => d.Account)
            .Include(d => d.Mechanic)
            .ThenInclude(d => d.Account)
            .Include(d => d.Operator)
            .ThenInclude(d => d.Account)
            .Include(d => d.Dispatcher)
            .ThenInclude(d => d.Account)
            .Include(d => d.Car)
            .Where(x => x.DispatcherId == dispatcher.Id)
            .OrderByDescending(x => x.Date)
            .AsQueryable();

        var dispatcherReviewDto = _mapper.Map<IEnumerable<DispatcherReviewDto>>(dispatcherHistories);

        return dispatcherReviewDto;
    }
}

