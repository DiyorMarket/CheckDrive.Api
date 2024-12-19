using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Application.DTOs.Review;

namespace CheckDrive.Application.Interfaces.Review;

public interface IReviewHistoryService
{
    Task<List<CheckPointDto>> GetDriverHistoriesAsync(int driverId);
    Task<List<DoctorReviewDto>> GetDoctorHistoriesAsync(int doctorId);
    Task<List<MechanicReviewHistoryDto>> GetMechanicHistoriesAsync(int mechanicId);
    Task<List<OperatorReviewHistory>> GetOperatorHistoriesAsync(int operatorId);
    Task<List<DispatcherReviewHistoryDto>> GetDispatcherHistoriesAsync(int dispatcherId);
    Task<List<DriverHistoryDto>> GetDriverReviewHistoriesAsync(int driverId);
}
