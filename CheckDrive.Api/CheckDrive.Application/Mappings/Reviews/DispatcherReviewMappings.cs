using AutoMapper;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class DispatcherReviewMappings : Profile
{
    public DispatcherReviewMappings()
    {
        CreateMap<DispatcherReview, DispatcherReviewDto>()
            .ForCtorParam(nameof(DispatcherReviewDto.DispatcherName), cfg => cfg.MapFrom(e => $"{e.Dispatcher.FirstName} {e.Dispatcher.LastName}"));

        CreateMap<DispatcherReview, DispatcherReviewHistoryDto>()
            .ForCtorParam(nameof(DispatcherReviewHistoryDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
            .ForCtorParam(nameof(DispatcherReviewHistoryDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"));
    }
}
