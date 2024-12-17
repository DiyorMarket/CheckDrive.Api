using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class ManagerReview : ReviewBase
{
    public int FinalMileage { get; set; }
    public decimal DebtAmount { get; set; }
    public decimal FuelConsumptionAmount { get; set; }
    public decimal RemainingFuelAmount { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int ManagerId { get; set; }
    public required virtual Manager Manager { get; set; }
}
