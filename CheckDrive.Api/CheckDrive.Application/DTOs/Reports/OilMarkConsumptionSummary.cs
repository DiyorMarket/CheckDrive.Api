namespace CheckDrive.Application.DTOs.Reports;

public class OilMarkConsumptionSummary
{
    public required string OilMark { get; set; }
    public decimal TotalFuelConsumed { get; set; }
    public decimal PreviousMonthConsumption { get; set; }
}
