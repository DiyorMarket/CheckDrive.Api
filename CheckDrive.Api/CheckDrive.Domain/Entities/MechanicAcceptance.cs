using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class MechanicAcceptance : ReviewBase
{
    public decimal FinalMileage { get; set; }
    public bool IsCarInGoodCondition { get; set; }
    public ReviewStatus Status { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int MechanicId { get; set; }
    public required virtual Mechanic Mechanic { get; set; }
}
