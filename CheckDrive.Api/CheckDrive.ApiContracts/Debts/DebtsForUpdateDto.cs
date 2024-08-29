namespace CheckDrive.ApiContracts.Debts
{
    public class DebtsForUpdateDto
    {
        public double OilAmount { get; set; }
        public StatusForDto Status { get; set; }

        public int DriverId { get; set; }
    }
}