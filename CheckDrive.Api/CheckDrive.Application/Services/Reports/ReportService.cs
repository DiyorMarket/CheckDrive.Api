using CheckDrive.Application.DTOs.Reports;
using CheckDrive.Application.Interfaces.Reports;
using CheckDrive.Domain.Interfaces;

namespace CheckDrive.Application.Services.Reports;

internal sealed class ReportService(IReportGenerator reportGenerator, ICheckDriveDbContext context) : IReportService
{
    public Task<FileStream> GenerateReportAsync(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> GenerateReportForCurrentMonthAsync()
    {
        var oilMarkSummary = GenerateRandomOilMarkData();
        var carUsageSummary = GenerateRandomCarUsageData(25);
        var reportSummary = new MonthlyReportSummary(1_000_000, 100_000_000, 5, oilMarkSummary, carUsageSummary);

        var report = reportGenerator.Generate(reportSummary);

        return Task.FromResult(report);
    }

    public static List<OilMarkConsumptionSummary> GenerateRandomOilMarkData()
    {
        var oilMarks = new[] { "80", "85", "90", "91", "95", "Diesel" };
        var random = new Random();
        var data = new List<OilMarkConsumptionSummary>();

        foreach (var mark in oilMarks)
        {
            data.Add(new OilMarkConsumptionSummary
            {
                OilMark = mark,
                TotalFuelConsumed = (decimal)(random.Next(500, 5000) / 10.0),
                PreviousMonthConsumption = (decimal)(random.Next(400, 4500) / 10.0)
            });
        }
        return data;
    }

    public static List<CarUsageReport> GenerateRandomCarUsageData(int count)
    {
        var random = new Random();
        var oilMarks = new[] { "80", "85", "90", "91", "95", "Diesel" };
        var data = new List<CarUsageReport>();

        for (int i = 0; i < count; i++)
        {
            var fuelConsumed = (decimal)(random.Next(500, 2000) / 10.0);
            data.Add(new CarUsageReport
            {
                CarName = $"Car {i + 1}",
                AverageFuelConsumptionLimit = (decimal)(random.Next(5, 15)),
                TotalFuelAdded = fuelConsumed + (decimal)random.Next(10, 50),
                ActualFuelConsumed = fuelConsumed,
                OilMark = oilMarks[random.Next(oilMarks.Length)],
                TotalDistanceTraveled = random.Next(100, 1000),
                ExpectedFuelConsumption = fuelConsumed,
                StartingFuelLeftover = (decimal)random.Next(5, 50),
                EndingFuelLeftover = (decimal)random.Next(5, 50)
            });
        }
        return data;
    }
}
