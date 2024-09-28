using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class MechanicAcceptance : ReviewBase
{
    public int FinalMileage { get; set; }
    public decimal RemainingFuelAmount { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int MechanicId { get; set; }
    public required virtual Mechanic Mechanic { get; set; }
}
