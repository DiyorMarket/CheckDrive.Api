using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class OilMarks : EntityBase
    {
        public string OilMark { get; set; }
        public virtual ICollection<OperatorReview> OperatorReviews { get; set; }
    }
}
