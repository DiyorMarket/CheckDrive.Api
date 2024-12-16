using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class Driver : Employee
{
    public DriverStatus Status { get; set; }

    public int? AssignedCarId { get; set; }
    public Car? AssignedCar { get; set; }

    public virtual ICollection<DoctorReview> Reviews { get; set; }

    public Driver()
    {
        Reviews = [];
    }
}
