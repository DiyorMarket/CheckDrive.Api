using CheckDrive.Application.DTOs.Dashboard;
using CheckDrive.Application.Interfaces;
using CheckDrive.Domain.Enums;
using CheckDrive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CheckDrive.Application.Services;

internal sealed class DashboardService : IDashboardService
{
    private readonly ICheckDriveDbContext _context;

    public DashboardService(ICheckDriveDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var carsSummary = await GetCarsSummaryAsync();
        var employeeSummaries = await GetEmployeeSummariesAsync();
        var oilConsumptionSummaries = await GetOilConsumptionSummariesAsync();
        var checkPointsSummaries = await GetCheckPointsSummariesAsync();

        return new DashboardDto(
            carsSummary,
            employeeSummaries,
            oilConsumptionSummaries,
            checkPointsSummaries);
    }

    private async Task<CarsSummary> GetCarsSummaryAsync()
    {
        var cars = await _context.Cars.ToListAsync();
        var freeCarsCount = cars.Count(x => x.Status == CarStatus.Free);
        var outOfServiceCarsCount = cars.Count(x => x.Status == CarStatus.OutOfService);
        var busyCarsCount = cars.Count(x => x.Status == CarStatus.Free);

        return new CarsSummary(freeCarsCount, outOfServiceCarsCount, busyCarsCount);
    }

    private async Task<List<EmployeeSummary>> GetEmployeeSummariesAsync()
    {
        var employeesByRole = await _context.Employees
            .GroupBy(x => x.Position)
            .Where(x => x.Key != EmployeePosition.Base && x.Key != EmployeePosition.Manager && x.Key != EmployeePosition.Custom)
            .Select(x => new EmployeeSummary(x.Key.ToString(), x.Count()))
            .ToListAsync();

        return employeesByRole;
    }

    private async Task<List<OilConsumptionSummary>> GetOilConsumptionSummariesAsync()
    {
        return new List<OilConsumptionSummary>()
        {
            new OilConsumptionSummary("September", 50, 40, 35, 40, 50),
            new OilConsumptionSummary("October", 25, 67, 65, 15, 32),
            new OilConsumptionSummary("November", 30, 20, 35, 45, 12),
            new OilConsumptionSummary("December", 45, 17, 45, 55, 70),
        };
    }

    private async Task<List<CheckPointSummary>> GetCheckPointsSummariesAsync()
    {
        var checkPoints = await _context.CheckPoints
                .Where(x => x.StartDate.Day == DateTime.UtcNow.Day)
                .Select(x => new CheckPointSummary(
                    x.Id,
                    x.StartDate,
                    x.DoctorReview.Driver.FirstName + " " + x.DoctorReview.Driver.LastName,
                    x.MechanicHandover != null ? x.MechanicHandover.Car.Model : "",
                    x.Stage,
                    x.Status))
                .ToListAsync();

        return checkPoints;
    }
}
