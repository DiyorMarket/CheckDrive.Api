using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class MechanicHandover : ReviewBase
{
    public decimal InitialMileage { get; set; }
    public ReviewStatus Status { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int CarId { get; set; }
    public required virtual Car Car { get; set; }

    public int MechanicId { get; set; }
    public required virtual Mechanic Mechanic { get; set; }
}
