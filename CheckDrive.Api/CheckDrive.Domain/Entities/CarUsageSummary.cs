namespace CheckDrive.Domain.Entities;

public class CarUsageSummary
{
    public int CurrentMonthDistance { get; set; }
    public int CurrentYearDistance { get; set; }
    public decimal CurrentMonthFuelConsumption { get; set; }
    public decimal CurrentYearFuelConsumption { get; set; }
}
