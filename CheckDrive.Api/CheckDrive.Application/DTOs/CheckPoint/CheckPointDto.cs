using CheckDrive.Application.DTOs.Car;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.DTOs.ManagerReview;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.DTOs.CheckPoint;

public sealed class CheckPointDto
{
    public CheckPointDto()
    {

    }

    public int Id { get; init; }
    public DateTime StartDate { get; init; }
    public CheckPointStatus Status { get; init; }
    public CheckPointStage Stage { get; init; }
    public DoctorReviewDto DoctorReview { get; init; }
    public MechanicHandoverReviewDto? MechanicHandover { get; init; }
    public OperatorReviewDto? OperatorReview { get; init; }
    public MechanicAcceptanceReviewDto? MechanicAcceptance { get; init; }
    public DispatcherReviewDto? DispatcherReview { get; init; }
    public ManagerReviewDto? ManagerReview { get; init; }
    public DebtDto? Debt { get; init; }
    public DriverDto Driver { get; init; }
    public CarDto Car { get; init; }
}