using System;

namespace CheckDrive.ApiContracts.Debts
{
    public class DebtsDto
    {
        public int Id { get; set; }
        public double OilAmount { get; set; }
        public StatusForDto Status { get; set; }
        public DateTime Date { get; set; }

        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; }
    }
}
