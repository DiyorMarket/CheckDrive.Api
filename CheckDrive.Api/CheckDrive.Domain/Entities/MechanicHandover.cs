using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities.Identity;

namespace CheckDrive.Domain.Entities;

public class MechanicHandover : ReviewBase
{
    public int InitialMileage { get; set; }

    public int CheckPointId { get; set; }
    public required CheckPoint CheckPoint { get; set; }

    public int MechanicId { get; set; }
    public required User Mechanic { get; set; }
}
