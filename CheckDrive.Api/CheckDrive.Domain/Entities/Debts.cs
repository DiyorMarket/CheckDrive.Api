using CheckDrive.Domain.Common;
using Microsoft.Identity.Client;

namespace CheckDrive.Domain.Entities
{
    public class Debts : EntityBase
    {
        public double OilAmount { get; set; }
        public Status Status { get; set; }

        public Driver Driver { get; set; }
        public int DriverId { get; set; }
    }
}
