using AutoMapper;
using CheckDrive.Application.DTOs.DoctorReview;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Mappings;

internal sealed class CheckPointMappings : Profile
{
    public CheckPointMappings()
    {
        CreateMap<CreateDoctorReviewDto, CheckPoint>()
            .ForMember(x => x.Status, cfg => cfg.MapFrom(d => d.IsApprovedByReviewer ? CheckPointStatus.InProgress : CheckPointStatus.InterruptedByReviewerRejection))
            .ForMember(x => x.Stage, cfg => cfg.MapFrom(_ => CheckPointStage.DoctorReview))
            .ForMember(x => x.StartDate, cfg => cfg.MapFrom(_ => DateTime.UtcNow));
    }
}
