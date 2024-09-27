using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class Dispatcher : Employee
{
    public virtual ICollection<DispatcherReview> Reviews { get; set; }

    public Dispatcher()
    {
        Reviews = new HashSet<DispatcherReview>();
    }
}
