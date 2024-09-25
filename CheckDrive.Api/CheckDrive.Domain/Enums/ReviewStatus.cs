namespace CheckDrive.Domain.Enums;

public enum ReviewStatus
{
    Approved = 0,
    PendingDriverApproval = 1,
    RejectedByReviewer = 2,
    RejectedByDriver = 3,
}
