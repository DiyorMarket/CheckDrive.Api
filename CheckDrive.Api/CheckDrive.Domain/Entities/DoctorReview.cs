using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities.Identity;

namespace CheckDrive.Domain.Entities;

public class DoctorReview : ReviewBase
{
    public int CheckPointId { get; set; }
    public required CheckPoint CheckPoint { get; set; }

    public Guid DoctorId { get; set; }
    public required User Doctor { get; set; }
}
