using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class Doctor : Employee
{
    public virtual ICollection<DoctorReview> Reviews { get; set; }

    public Doctor()
    {
        Reviews = new HashSet<DoctorReview>();
    }
}
