using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.DTOs.MechanicHandover;

public class MechanicHandoverReviewDto : ReviewDtoBase
{
    public int InitialMileage { get; set; }
}
