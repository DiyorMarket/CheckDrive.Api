namespace CheckDrive.ApiContracts.Debts
{
    public class DebtsDto
    {
        public double OilAmount { get; set; }
        public StatusForDto Status { get; set; }

        public int DriverId { get; set; }
    }
}
