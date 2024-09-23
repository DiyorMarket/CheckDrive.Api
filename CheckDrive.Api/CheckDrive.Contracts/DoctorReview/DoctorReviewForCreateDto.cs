using System;

namespace CheckDrive.ApiContracts.DoctorReview
{
    public class DoctorReviewForCreateDto
    {
        public int DriverId { get; set; }
        public string Notes { get; set; }
        public DateTime Date { get; set; }
        public ReviewStatus Status { get; set; }
    }
}
