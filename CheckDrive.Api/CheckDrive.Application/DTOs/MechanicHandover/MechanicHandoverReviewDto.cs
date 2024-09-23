using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public class MechanicHandoverReviewDto : ReviewDto
{
    public int InitialMileage { get; set; }
}
