using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class DoctorReviewMappings : Profile
{
    public DoctorReviewMappings()
    {
        CreateMap<DoctorReview, DoctorReviewDto>()
            .ForCtorParam(nameof(DoctorReviewDto.DoctorName), cfg => cfg.MapFrom(e => $"{e.Doctor.FirstName} {e.Doctor.LastName}"))
            .ForCtorParam(nameof(DoctorReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.Driver.FirstName} {e.Driver.LastName}"));

        CreateMap<CreateDoctorReviewDto, DoctorReview>()
            .ForMember(x => x.Date, cfg => cfg.MapFrom(_ => DateTime.UtcNow));
    }
}
