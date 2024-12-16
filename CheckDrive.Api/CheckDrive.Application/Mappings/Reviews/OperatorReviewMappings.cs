using AutoMapper;
using CheckDrive.Application.DTOs.OperatorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class OperatorReviewMappings : Profile
{
    public OperatorReviewMappings()
    {
        CreateMap<OperatorReview, OperatorReviewDto>()
            .ForCtorParam(nameof(OperatorReviewDto.OperatorName), cfg => cfg.MapFrom(e => $"{e.Operator.FirstName} {e.Operator.LastName}"));

        CreateMap<OperatorReview, OperatorReviewHistory>()
            .ForCtorParam(nameof(OperatorReviewHistory.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
            .ForCtorParam(nameof(OperatorReviewHistory.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(OperatorReviewHistory.OilMarkName), cfg => cfg.MapFrom(e => e.OilMark.Name));

        CreateMap<CreateOperatorReviewDto, OperatorReview>();
    }
}
