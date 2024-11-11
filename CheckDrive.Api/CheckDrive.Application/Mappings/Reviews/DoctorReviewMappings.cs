using AutoMapper;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class DoctorReviewMappings : Profile
{
    public DoctorReviewMappings()
    {
        //CreateMap<DoctorReview, DoctorReviewDto>()
        //    .ForCtorParam(nameof(DoctorReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.Driver.FirstName} {e.Driver.LastName}"))
        //    .ForCtorParam(nameof(DoctorReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Doctor.FirstName} {e.Doctor.LastName}"));
    }
}
