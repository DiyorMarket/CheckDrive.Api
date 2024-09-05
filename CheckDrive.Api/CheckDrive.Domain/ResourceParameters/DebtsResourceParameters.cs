using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.ResourceParameters
{
    public class DebtsResourceParameters : ResourceParametersBase
    {
        public double? OilAmount { get; set; }
        public double? OilAmountLessThan { get; set; }
        public double? OilAmountGreaterThan { get; set; }
        public Status? Status { get; set; }
        public int? DriverId { get; set; }
        public override string OrderBy { get; set; } = "oilAmount";
    }
}
