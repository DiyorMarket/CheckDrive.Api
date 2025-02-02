namespace CheckDrive.Application.DTOs.Reports;

public sealed class CarUsageReport
{
    public string CarName { get; set; }
    public string OilMark { get; set; }
    public decimal AverageFuelConsumptionLimit { get; set; }
    public decimal TotalFuelAdded { get; set; }
    public decimal ActualFuelConsumed { get; set; }
    public int TotalDistanceTraveled { get; set; }
    public decimal ExpectedFuelConsumption { get; set; }
    public decimal StartingFuelLeftover { get; set; }
    public decimal EndingFuelLeftover { get; set; }

    public CarUsageReport()
    {
        CarName = string.Empty;
        OilMark = string.Empty;
    }
}
