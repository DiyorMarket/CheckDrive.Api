﻿namespace CheckDrive.Domain.Enums;

public enum CheckPointStatus
{
    InProgress,
    Completed,
    AutomaticallyClosed,
    PendingManagerReview,
    InterruptedByReviewerRejection,
    InterruptedByDriverRejection
}