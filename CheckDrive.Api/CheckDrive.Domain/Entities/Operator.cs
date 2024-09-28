using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class Operator : Employee
{
    public virtual ICollection<OperatorReview> Reviews { get; set; }

    public Operator()
    {
        Reviews = new HashSet<OperatorReview>();
    }
}
