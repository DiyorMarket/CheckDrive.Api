using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class DispatcherReview : ReviewBase
{
    public decimal? FuelConsumptionAdjustment { get; set; }
    public decimal? FinalMileageAdjustment { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int DispatcherId { get; set; }
    public required virtual Dispatcher Dispatcher { get; set; }
}