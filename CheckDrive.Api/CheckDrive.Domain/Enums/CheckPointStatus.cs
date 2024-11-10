namespace CheckDrive.Domain.Enums;

public enum CheckPointStatus
{
    InProgress = 0,
    Completed = 1,
    InterruptedByReviewerRejection = 2,
    InterruptedByDriverRejection = 3,
    AutomaticallyClosed = 4,
    PendingManagerReview = 5,
    ClosedByManager = 6,
}
