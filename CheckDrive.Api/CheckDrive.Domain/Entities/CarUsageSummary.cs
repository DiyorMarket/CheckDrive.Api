namespace CheckDrive.Domain.Entities;

public class CarUsageSummary
{
    public decimal CurrentMonthDistance { get; set; }
    public decimal CurrentYearDistance { get; set; }
    public decimal CurrentMonthFuelConsumption { get; set; }
    public decimal CurrentYearFuelConsumption { get; set; }
}
