namespace CheckDrive.Domain.Entities;

public class Driver : Employee
{
    public int? AssignedCarId { get; set; }
    public Car? AssignedCar { get; set; }
    public bool IsDriverAvailableForReview { get; set; }

    public virtual ICollection<DoctorReview> Reviews { get; set; }

    public Driver()
    {
        Reviews = new HashSet<DoctorReview>();
    }
}
