using AutoMapper;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class OperatorReviewMappings : Profile
{
    public OperatorReviewMappings()
    {
        CreateMap<OperatorReview, OperatorReviewDto>()
            .ForCtorParam(nameof(OperatorReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
            .ForCtorParam(nameof(OperatorReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(OperatorReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.OperatorId))
            .ForCtorParam(nameof(OperatorReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Operator.FirstName} {e.Operator.LastName}"));
    }
}
