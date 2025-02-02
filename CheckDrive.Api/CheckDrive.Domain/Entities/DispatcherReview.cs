using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class DispatcherReview : ReviewBase
{
    public decimal FinalMileage { get; set; }
    public decimal FuelConsumptionAmount { get; set; }
    public decimal RemainingFuelAmount { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int DispatcherId { get; set; }
    public required virtual Dispatcher Dispatcher { get; set; }
}