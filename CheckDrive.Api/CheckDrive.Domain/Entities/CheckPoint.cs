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
    public required User Driver { get; set; }

    public int? CarId { get; set; }
    public Car? Car { get; set; }

    public int DoctorReviewId { get; set; }
    public required DoctorReview DoctorReview { get; set; }

    public int? MechanicHandoverId { get; set; }
    public MechanicHandover? MechanicHandover { get; set; }

    public int? OperatorReviewId { get; set; }
    public OperatorReview? OperatorReview { get; set; }

    public int? MechanicAcceptanceId { get; set; }
    public MechanicAcceptance? MechanicAcceptance { get; set; }

    public int? DispatcherReviewId { get; set; }
    public DispatcherReview? DispatcherReview { get; set; }

    public int? DebtId { get; set; }
    public Debt? Debt { get; set; }
}
