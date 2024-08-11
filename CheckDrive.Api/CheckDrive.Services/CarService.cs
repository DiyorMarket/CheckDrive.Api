using AutoMapper;
using CheckDrive.Api.Extensions;
using CheckDrive.ApiContracts;
using CheckDrive.ApiContracts.Car;
using CheckDrive.ApiContracts.DispatcherReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Services;
using CheckDrive.Domain.Pagniation;
using CheckDrive.Domain.ResourceParameters;
using CheckDrive.Domain.Responses;
using CheckDrive.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;

namespace CheckDrive.Services;

public class CarService : ICarService
{
    private readonly IMapper _mapper;
    private readonly CheckDriveDbContext _context;

    public CarService(IMapper mapper, CheckDriveDbContext context)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<GetBaseResponse<CarDto>> GetCarsAsync(CarResourceParameters resourceParameters)
    {
        var query = GetQueryCarResParameters(resourceParameters);

        if (resourceParameters.RoleId == 1)
        {
            var countOfCars = query.Count();
            resourceParameters.MaxPageSize = countOfCars;
            resourceParameters.PageSize = countOfCars;
        }

        var cars = await query.ToPaginatedListAsync(resourceParameters.PageSize, resourceParameters.PageNumber);

        var carDtos = _mapper.Map<List<CarDto>>(cars);

        var paginatedResult = new PaginatedList<CarDto>(carDtos, cars.TotalCount, cars.CurrentPage, cars.PageSize);

        return paginatedResult.ToResponse();
    }

    public async Task<CarDto?> GetCarByIdAsync(int id)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id);

        var carDto = _mapper.Map<CarDto>(car);

        return carDto;
    }

    public async Task<CarDto> CreateCarAsync(CarForCreateDto carForCreate)
    {
        var carEntity = _mapper.Map<Car>(carForCreate);

        await _context.Cars.AddAsync(carEntity);
        await _context.SaveChangesAsync();

        var carDto = _mapper.Map<CarDto>(carEntity);

        return carDto;
    }

    public async Task<CarDto> UpdateCarAsync(CarForUpdateDto carForUpdate)
    {
        var carEntity = _mapper.Map<Car>(carForUpdate);

        _context.Cars.Update(carEntity);
        await _context.SaveChangesAsync();

        var carDto = _mapper.Map<CarDto>(carEntity);

        return carDto;
    }

    public async Task DeleteCarAsync(int id)
    {
        var car = await _context.Cars
            .Include(x => x.Reviewers)
            .Include(mh => mh.MechanicHandovers)
            .Include(ma => ma.MechanicAcceptance)
            .Include(o => o.OperatorReviews)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (car is not null)
        {
            _context.MechanicsHandovers.RemoveRange(car.MechanicHandovers);
            _context.MechanicsAcceptances.RemoveRange(car.MechanicAcceptance);
            _context.OperatorReviews.RemoveRange(car.OperatorReviews);
            _context.DispatchersReviews.RemoveRange(car.Reviewers);
            _context.Cars.Remove(car);
        }
        await _context.SaveChangesAsync();
    }

    private IQueryable<Car> GetQueryCarResParameters(
           CarResourceParameters resourceParameters)
    {
        var query = _context.Cars.
            AsNoTracking().
            AsQueryable();

        if (!string.IsNullOrWhiteSpace(resourceParameters.SearchString))
        {
            query = query.Where(x => x.Model.Contains(resourceParameters.SearchString)
            || x.Color.Contains(resourceParameters.SearchString));
        }

        if (resourceParameters.IsBusy is not null)
        {
            query = query.Where(x => x.isBusy == resourceParameters.IsBusy);
        }
        //MeduimFuelConsumption
        if (resourceParameters.MeduimFuelConsumption is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption == resourceParameters.MeduimFuelConsumption);
        }
        if (resourceParameters.ManufacturedYearLessThan is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption < resourceParameters.MeduimFuelConsumptionLessThan);
        }
        if (resourceParameters.MeduimFuelConsumptionGreaterThan is not null)
        {
            query = query.Where(x => x.MeduimFuelConsumption > resourceParameters.MeduimFuelConsumptionGreaterThan);
        }

        //FuelTankCapacity
        if (resourceParameters.FuelTankCapacity is not null)
        {
            query = query.Where(x => x.FuelTankCapacity == resourceParameters.FuelTankCapacity);
        }
        if (resourceParameters.FuelTankCapacityLessThan is not null)
        {
            query = query.Where(x => x.FuelTankCapacity < resourceParameters.FuelTankCapacityLessThan);
        }
        if (resourceParameters.FuelTankCapacityThan is not null)
        {
            query = query.Where(x => x.FuelTankCapacity > resourceParameters.FuelTankCapacityThan);
        }

        //ManufacturedYear
        if (resourceParameters.ManufacturedYear is not null)
        {
            query = query.Where(x => x.ManufacturedYear == resourceParameters.ManufacturedYear);
        }
        if (resourceParameters.ManufacturedYearLessThan is not null)
        {
            query = query.Where(x => x.ManufacturedYear < resourceParameters.ManufacturedYearLessThan);
        }
        if (resourceParameters.ManufacturedYearGreaterThan is not null)
        {
            query = query.Where(x => x.ManufacturedYear > resourceParameters.ManufacturedYearLessThan);
        }

        return query;
    }

    public async Task<IEnumerable<CarHistoryDto>> GetCarHistories(int year, int month)
    {
        var date = DateTime.Today.ToTashkentTime().Date;

        var dispatcherResponse = await _context.DispatchersReviews
            .AsNoTracking()
            .Where(x => x.Date.Month == month && x.Date.Year == year)
            .Include(ma => ma.MechanicAcceptance)
            .Include(mh => mh.MechanicHandover)
            .Include(d => d.Car)
            .ToListAsync();

        var cars = await _context.Cars.ToListAsync();

        List<CarHistoryDto> carHistory = new List<CarHistoryDto>();

        foreach (var car in cars)
        {
            var totalDistanceCovered = dispatcherResponse
            .Where(x => x.CarId == car.Id)
            .Sum(x => x.DistanceCovered);

            var dispatcherReview = dispatcherResponse
                .Where(x => x.CarId == car.Id)
                .OrderByDescending(x => x.Date)
                .First();

            var dispatcherReviewDto = _mapper.Map<DispatcherReviewDto>(dispatcherReview);

            carHistory.Add(new CarHistoryDto
            {
                Model = car.Model,
                Number = car.Number,
                MonthlyMediumDistance = car.OneYearMediumDistance / 12,
                MonthlyMileage = (int)totalDistanceCovered,
                MonthlyNormalOilSpend = (car.OneYearMediumDistance / 12) * car.MeduimFuelConsumption / 100,
                MonthlySpentOil = totalDistanceCovered * car.MeduimFuelConsumption / 100,
                MonthlyRefueledOil = ((car.OneYearMediumDistance / 12) * car.MeduimFuelConsumption / 100) - (totalDistanceCovered * car.MeduimFuelConsumption / 100),
                RemainingFuel = dispatcherReviewDto.RemainigFuelAfter,
            });
        }

        return carHistory;
    }

    public async Task<byte[]> MonthlyExcelData(PropertyForExportFile propertyForExportFile)
    {
        var cars = await GetCarHistories(propertyForExportFile.Year, propertyForExportFile.Month);

        if (cars == null) return null;

        using (ExcelEngine excel = new ExcelEngine())
        {
            IApplication application = excel.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;

            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            // Adding title
            worksheet.Range["A1:K1"].Merge();
            worksheet.Range["A1"].Text = $"Информация автомобилях на эту дату {propertyForExportFile.Month}.{propertyForExportFile.Year}";
            worksheet.Range["A1"].CellStyle.Font.Bold = true;
            worksheet.Range["A1"].CellStyle.Font.Size = 16;
            worksheet.Range["A1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

            // Adding headers
            worksheet.Range["A2"].Text = "Имя автомобиля";
            worksheet.Range["B2"].Text = "Номер автомобиля";
            worksheet.Range["C2"].Text = "Ежемесячно средняя дистанция";
            worksheet.Range["D2"].Text = "Ежемесячный пробег";
            worksheet.Range["E2"].Text = "Ежемесячные нормальные расходы топлива";
            worksheet.Range["F2"].Text = "Ежемесячный расходы топлива";
            worksheet.Range["G2"].Text = "Ежемесячно заправляемое топлива";
            worksheet.Range["H2"].Text = "Оставшееся топливо";

            // Настройка ширины столбцов
            worksheet.Range["A2:H2"].CellStyle.Font.Bold = true;
            worksheet.Columns[0].ColumnWidth = 20; // MechanicName
            worksheet.Columns[1].ColumnWidth = 20; // DriverName
            worksheet.Columns[2].ColumnWidth = 20; // CarName
            worksheet.Columns[3].ColumnWidth = 20; // Distance
            worksheet.Columns[4].ColumnWidth = 20; // Date
            worksheet.Columns[5].ColumnWidth = 20; // IsHanded
            worksheet.Columns[6].ColumnWidth = 20; // Status
            worksheet.Columns[7].ColumnWidth = 20; // Comments

            int row = 3;
            foreach (var car in cars)
            {
                worksheet.Range["A" + row].Text = car.Model;
                worksheet.Range["B" + row].Text = car.Number;
                worksheet.Range["C" + row].Number = car.MonthlyMediumDistance;
                worksheet.Range["D" + row].Number = car.MonthlyMileage;
                worksheet.Range["E" + row].Number = car.MonthlyNormalOilSpend;
                worksheet.Range["F" + row].Number = car.MonthlySpentOil;
                worksheet.Range["G" + row].Number = car.MonthlyRefueledOil;
                worksheet.Range["H" + row].Number = car.RemainingFuel;
                row++;
            }

            // Save the workbook to a memory stream
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
}

