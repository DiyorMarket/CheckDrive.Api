using AutoMapper;
using CheckDrive.Application.DTOs.ManagerReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class ManagerReviewMappings : Profile
{
    public ManagerReviewMappings()
    {
        CreateMap<ManagerReview, ManagerReviewDto>()
            .ForCtorParam(nameof(ManagerReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Manager.FirstName} {e.Manager.LastName}"))
            .ForCtorParam(nameof(ManagerReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(ManagerReviewDto.DriverId), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.Id}"))
            .ForCtorParam(nameof(ManagerReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.ManagerId))
            .ForCtorParam(nameof(ManagerReviewDto.InitialMillage), cfg => cfg.MapFrom(e => e.InitialMileage))
            .ForCtorParam(nameof(ManagerReviewDto.RemainingFuelAmount), cfg => cfg.MapFrom(e => e.RemainingFuelAmount))
            .ForCtorParam(nameof(ManagerReviewDto.FuelConsumptionAmount), cfg => cfg.MapFrom(e => e.FuelConsumptionAmount))
            .ForCtorParam(nameof(ManagerReviewDto.DebtAmount), cfg => cfg.MapFrom(e => e.DebtAmount))
            .ForCtorParam(nameof(ManagerReviewDto.FinalMileage), cfg => cfg.MapFrom(e => e.FinalMileage))
            .ForCtorParam(nameof(ManagerReviewDto.Date), cfg => cfg.MapFrom(e => e.Date))
            .ForCtorParam(nameof(ManagerReviewDto.Notes), cfg => cfg.MapFrom(e => e.Notes));

    }
}
