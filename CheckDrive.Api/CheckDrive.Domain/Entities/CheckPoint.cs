using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class CheckPoint : EntityBase
{
    public CheckPointStatus Status { get; set; }
    public CheckPointStage Stage { get; set; }

    public required virtual DoctorReview DoctorReview { get; set; }
    public virtual MechanicHandover? MechanicHandover { get; set; }
    public virtual OperatorReview? OperatorReview { get; set; }
    public virtual MechanicAcceptance? MechanicAcceptance { get; set; }
    public virtual DispatcherReview? DispatcherReview { get; set; }
    public virtual Debt? Debt { get; set; }
}
