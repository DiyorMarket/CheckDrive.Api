namespace CheckDrive.Domain.Entities;

public class CarLimits
{
    public int MonthlyDistanceLimit { get; set; }
    public int YearlyDistanceLimit { get; set; }
    public decimal MonthlyFuelConsumptionLimit { get; set; }
    public decimal YearlyFuelConsumptionLimit { get; set; }
}
