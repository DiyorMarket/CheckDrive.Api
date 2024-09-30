using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;
public class Manager : Employee
{
    public virtual ICollection<ManagerReview> Reviews { get; set; }

    public Manager()
    {
        Reviews = new HashSet<ManagerReview>();
    }
}
