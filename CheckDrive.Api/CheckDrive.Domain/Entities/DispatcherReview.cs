using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities.Identity;

namespace CheckDrive.Domain.Entities;

public class DispatcherReview : ReviewBase
{
    public decimal? FuelConsumptionAdjustment { get; set; }
    public decimal? DistanceTravelledAdjustment { get; set; }

    public int CheckPointId { get; set; }
    public required CheckPoint CheckPoint { get; set; }

    public int DispatcherId { get; set; }
    public required User Dispatcher { get; set; }
}