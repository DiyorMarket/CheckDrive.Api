namespace CheckDrive.ApiContracts.Debts
{
    public class DebtsDto
    {
        public int Id { get; set; }
        public double OilAmount { get; set; }
        public StatusForDto Status { get; set; }

        public int DriverId { get; set; }
        public int CarId { get; set; }
    }
}
