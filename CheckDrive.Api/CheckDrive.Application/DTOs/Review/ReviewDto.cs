using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.Review;

public class ReviewDto
{
    public int CheckPointId { get; set; }
    public Guid ReviewerId { get; set; }
    public string ReviewerName { get; set; }
    public Guid DriverId { get; set; }
    public string DriverName { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public ReviewStatus Status { get; set; }
    public ReviewType Type { get; set; }
}
