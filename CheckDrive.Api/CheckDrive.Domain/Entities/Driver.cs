using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class Driver : Employee
{
    public virtual ICollection<DoctorReview> Reviews { get; set; }

    public Driver()
    {
        Reviews = new HashSet<DoctorReview>();
    }
}
