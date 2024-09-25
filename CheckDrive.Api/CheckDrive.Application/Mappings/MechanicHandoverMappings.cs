using AutoMapper;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Application.Mappings;

internal sealed class MechanicHandoverMappings : Profile
{
    public MechanicHandoverMappings()
    {
        CreateMap<CreateMechanicHandoverReviewDto, MechanicHandover>()
            .ForMember(x => x.Date, cfg => cfg.MapFrom(_ => DateTime.UtcNow))
            .ForMember(x => x.Status, cfg => cfg.MapFrom(x => x.IsApprovedByReviewer ? ReviewStatus.PendingDriverApproval : ReviewStatus.RejectedByReviewer));

        CreateMap<MechanicHandover, MechanicHandoverReviewDto>()
            .ForCtorParam(nameof(MechanicHandoverReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DriverId))
            .ForCtorParam(nameof(MechanicHandoverReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.Driver.FirstName} {e.CheckPoint.Driver.LastName}"))
            .ForCtorParam(nameof(MechanicHandoverReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.MechanicId))
            .ForCtorParam(nameof(MechanicHandoverReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Mechanic.FirstName} {e.Mechanic.LastName}"));
    }
}
