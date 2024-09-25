using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;

namespace CheckDrive.Application.Interfaces;

public interface IReviewService
{
    Task<DoctorReviewDto> CreateAsync(CreateDoctorReviewDto review);
    Task<MechanicHandoverReviewDto> CreateAsync(CreateMechanicHandoverDto review);
    Task<OperatorReviewDto> CreateAsync(CreateOperatorReviewDto review);
}
