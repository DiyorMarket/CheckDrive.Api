﻿namespace CheckDrive.Domain.Common;

public abstract class ReviewBase : EntityBase
{
    public string? Notes { get; set; }
    public DateTime Date { get; set; }
}
