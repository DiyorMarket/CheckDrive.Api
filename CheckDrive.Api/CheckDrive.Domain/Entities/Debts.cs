using CheckDrive.Domain.Common;

namespace CheckDrive.Domain.Entities
{
    public class Debts : EntityBase
    {
        public double OilAmount { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public int DispatcherReviewId {  get; set; }
        public Driver Driver { get; set; }
        public int DriverId { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
    }
}
