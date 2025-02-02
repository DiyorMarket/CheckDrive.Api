namespace CheckDrive.Application.DTOs.Reports;

public sealed class MonthlyReportSummary
{
    public decimal OutOfServiceCarsCount { get; set; }
    public decimal LimitsExceededCarsCount { get; set; }
    public int TotalAnomalies { get; set; }

    public List<OilMarkConsumptionSummary> OilMarkConsumptionSummaries { get; set; }
    public List<CarUsageReport> CarUsageSummaries { get; set; }

    public MonthlyReportSummary()
    {
        OilMarkConsumptionSummaries = [];
        CarUsageSummaries = [];
    }

    public MonthlyReportSummary(
        decimal outOfServiceCarsCount,
        decimal limitsExceededCarsCount,
        int totalAnomalies,
        List<OilMarkConsumptionSummary> oilMarkConsumptionSummaries,
        List<CarUsageReport> carUsageSummaries)
    {
        OutOfServiceCarsCount = outOfServiceCarsCount;
        LimitsExceededCarsCount = limitsExceededCarsCount;
        TotalAnomalies = totalAnomalies;
        OilMarkConsumptionSummaries = oilMarkConsumptionSummaries;
        CarUsageSummaries = carUsageSummaries;
    }
}
