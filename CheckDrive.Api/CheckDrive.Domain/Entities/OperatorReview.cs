﻿using CheckDrive.Domain.Common;
using CheckDrive.Domain.Entities.Identity;

namespace CheckDrive.Domain.Entities;

public class OperatorReview : ReviewBase
{
    public decimal InitialOilAmount { get; set; }
    public decimal OilRefillAmount { get; set; }

    public int CheckPointId { get; set; }
    public required CheckPoint CheckPoint { get; set; }

    public int OilMarkId { get; set; }
    public required OilMark OilMark { get; set; }

    public int OperatorId { get; set; }
    public required User Operator { get; set; }
}
