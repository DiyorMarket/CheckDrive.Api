using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class Debt : EntityBase
{
    public decimal FuelAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DebtStatus Status { get; set; }

    public int CheckPointId { get; set; }
    public required virtual CheckPoint CheckPoint { get; set; }

    public int? ManagerReviewId { get; set; }
    public virtual ManagerReview? ManagerReview { get; set; }
}
