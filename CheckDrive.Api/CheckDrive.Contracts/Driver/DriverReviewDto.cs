namespace CheckDrive.ApiContracts.Driver
{
    public class DriverReviewDto
    {
        public int ReviewId { get; set; }
        public string Notes { get; set; }
        public ReviewType Type { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
