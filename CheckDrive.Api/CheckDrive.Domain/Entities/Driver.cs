namespace CheckDrive.Domain.Entities;

public class Driver : Employee
{
    public bool CanStartCheckPointProcess { get; set; }

    public virtual ICollection<DoctorReview> Reviews { get; set; }

    public Driver()
    {
        Reviews = new HashSet<DoctorReview>();
    }
}
