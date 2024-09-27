using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Mappings;

internal sealed class DoctorReviewMappings : Profile
{
    public DoctorReviewMappings()
    {
        CreateMap<DoctorReview, DoctorReviewDto>()
            .ForCtorParam(nameof(DoctorReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
            .ForCtorParam(nameof(DoctorReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(DoctorReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.DoctorId))
            .ForCtorParam(nameof(DoctorReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Doctor.FirstName} {e.Doctor.LastName}"));

        CreateMap<CreateDoctorReviewDto, DoctorReview>()
            .ForMember(x => x.Date, cfg => cfg.MapFrom(_ => DateTime.UtcNow))
            .ForMember(x => x.Status, cfg => cfg.MapFrom(d => d.IsApprovedByReviewer ? ReviewStatus.Approved : ReviewStatus.RejectedByReviewer));
    }
}
