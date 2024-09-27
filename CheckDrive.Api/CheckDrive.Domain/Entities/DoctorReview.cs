using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class DoctorReview : ReviewBase
{
    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int DriverId { get; set; }
    public required virtual Driver Driver { get; set; }

    public int DoctorId { get; set; }
    public required virtual Doctor Doctor { get; set; }
}
