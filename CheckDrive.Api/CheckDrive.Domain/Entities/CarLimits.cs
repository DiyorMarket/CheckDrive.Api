namespace CheckDrive.Domain.Entities;

public class CarLimits
{
    public decimal MonthlyDistanceLimit { get; set; }
    public decimal YearlyDistanceLimit { get; set; }
    public decimal MonthlyFuelConsumptionLimit { get; set; }
    public decimal YearlyFuelConsumptionLimit { get; set; }
}
