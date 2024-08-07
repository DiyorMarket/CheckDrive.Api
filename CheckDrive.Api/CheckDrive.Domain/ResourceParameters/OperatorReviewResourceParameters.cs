﻿using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.ResourceParameters
{
    public class OperatorReviewResourceParameters : ResourceParametersBase
    {
        public bool? IsGiven { get; set; }
        public double? OilAmount { get; set; }
        public double? OilAmountLessThan { get; set; }
        public double? OilAmountGreaterThan { get; set; }
        public Status? Status { get; set; }
        public DateTime? Date { get; set; }
        public override string OrderBy { get; set; } = "id";
        public int? DriverId { get; set; }
        public int? CarId { get; set; }
        public int? RoleId { get; set; }
    }
}
