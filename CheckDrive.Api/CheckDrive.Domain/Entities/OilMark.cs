using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class OilMark : EntityBase
{
    public string Name { get; set; }

    public virtual ICollection<OperatorReview> Reviews { get; set; }

    public OilMark()
    {
        Reviews = new HashSet<OperatorReview>();
    }
}
