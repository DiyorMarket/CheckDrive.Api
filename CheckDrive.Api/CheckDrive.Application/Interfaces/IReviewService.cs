using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.MechanicHandover;

namespace CheckDrive.Application.Interfaces;

public interface IReviewService
{
    Task Create(CreateDoctorReviewDto review);
    Task Create(CreateMechanicHandoverDto review);
}
