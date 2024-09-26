using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities.Identity;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.Entities;

public class CheckPoint : EntityBase
{
    public DateTime StartDate { get; set; }
    public string? Notes { get; set; }
    public CheckPointStatus Status { get; set; }
    public CheckPointStage Stage { get; set; }

    public Guid DriverId { get; set; }
    public virtual User Driver { get; set; }

    public int DoctorReviewId { get; set; }
    public virtual DoctorReview DoctorReview { get; set; }

    public int? MechanicHandoverId { get; set; }
    public virtual MechanicHandover? MechanicHandover { get; set; }

    public int? OperatorReviewId { get; set; }
    public virtual OperatorReview? OperatorReview { get; set; }

    public int? MechanicAcceptanceId { get; set; }
    public virtual MechanicAcceptance? MechanicAcceptance { get; set; }

    public int? DispatcherReviewId { get; set; }
    public virtual DispatcherReview? DispatcherReview { get; set; }

    public int? DebtId { get; set; }
    public virtual Debt? Debt { get; set; }
}
