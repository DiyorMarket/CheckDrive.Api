using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.Hubs;

public interface IReviewHub
{
    Task NotifyDoctorReview(ReviewDtoBase review);
    Task MechanicHandoverConfirmation(MechanicHandoverReviewDto review);
    Task OperatorReviewConfirmation(OperatorReviewDto review);
    Task MechanicAcceptanceConfirmation(MechanicAcceptanceReviewDto review);
}
