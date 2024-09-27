using AutoMapper;
using CheckDrive.Application.DTOs.DispatcherReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class DispatcherReviewMappings : Profile
{
    public DispatcherReviewMappings()
    {
        CreateMap<DispatcherReview, DispatcherReviewDto>()
            .ForCtorParam(nameof(DispatcherReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
            .ForCtorParam(nameof(DispatcherReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(DispatcherReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.DispatcherId))
            .ForCtorParam(nameof(DispatcherReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Dispatcher.FirstName} {e.Dispatcher.LastName}"));
    }
}
