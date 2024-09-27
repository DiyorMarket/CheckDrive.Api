using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities;

public class OperatorReview : ReviewBase
{
    public decimal InitialOilAmount { get; set; }
    public decimal OilRefillAmount { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int OilMarkId { get; set; }
    public required virtual OilMark OilMark { get; set; }

    public int OperatorId { get; set; }
    public required virtual Operator Operator { get; set; }
}
