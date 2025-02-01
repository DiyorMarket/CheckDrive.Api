using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class ManagerReview : ReviewBase
{
    public required decimal InitialMileage { get; set; }
    public required decimal FinalMileage { get; set; }
    public required decimal DebtAmount { get; set; }
    public required decimal FuelConsumptionAmount { get; set; }
    public required decimal RemainingFuelAmount { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int ManagerId { get; set; }
    public required virtual Manager Manager { get; set; }
}
