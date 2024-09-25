﻿using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.OperatorReview;

public class OperatorReviewDto : ReviewDtoBase
{
    public double InitialOilAmount { get; set; }
    public double OilRefillAmount { get; set; }
    public string OilMark { get; set; }
}
