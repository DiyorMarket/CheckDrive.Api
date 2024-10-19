namespace CheckDrive.Domain.Entities;

public class Driver : Employee
{
    public bool CanStartToReview { get; set; }

    public virtual ICollection<DoctorReview> Reviews { get; set; }
    public Driver()
    {
        Reviews = new HashSet<DoctorReview>();
    }
}
