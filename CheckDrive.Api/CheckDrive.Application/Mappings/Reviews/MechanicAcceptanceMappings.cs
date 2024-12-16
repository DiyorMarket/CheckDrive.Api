using AutoMapper;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class MechanicAcceptanceMappings : Profile
{
    public MechanicAcceptanceMappings()
    {
        CreateMap<MechanicAcceptance, MechanicAcceptanceReviewDto>()
            .ForCtorParam(nameof(MechanicAcceptanceReviewDto.MechanicName), cfg => cfg.MapFrom(e => $"{e.Mechanic.FirstName} {e.Mechanic.LastName}"));

        CreateMap<CreateMechanicAcceptanceReviewDto, MechanicAcceptance>();
    }
}
