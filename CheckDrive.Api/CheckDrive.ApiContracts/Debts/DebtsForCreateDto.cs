namespace CheckDrive.ApiContracts.Debts
{
    public class DebtsForCreateDto
    {
        public double OilAmount { get; set; }
        public StatusForDto Status { get; set; }

        public int DriverId { get; set; }
        public int CarId { get; set; }
    }
}
