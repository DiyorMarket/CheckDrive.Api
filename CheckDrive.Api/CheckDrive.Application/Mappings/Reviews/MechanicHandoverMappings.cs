using AutoMapper;
using CheckDrive.Application.DTOs.MechanicHandover;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class MechanicHandoverMappings : Profile
{
    public MechanicHandoverMappings()
    {
        CreateMap<CreateMechanicHandoverReviewDto, MechanicHandover>()
            .ForMember(x => x.Date, cfg => cfg.MapFrom(_ => DateTime.UtcNow));

        CreateMap<MechanicHandover, MechanicHandoverReviewDto>()
            .ForCtorParam(nameof(MechanicHandoverReviewDto.MechanicName), cfg => cfg.MapFrom(e => $"{e.Mechanic.FirstName} {e.Mechanic.LastName}"));
    }
}
