using AutoMapper;
using CheckDrive.Application.DTOs.MechanicAcceptance;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class MechanicAcceptanceMappings : Profile
{
    public MechanicAcceptanceMappings()
    {
        CreateMap<MechanicAcceptance, MechanicAcceptanceReviewDto>()
            .ForCtorParam(nameof(MechanicAcceptanceReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DriverId))
            .ForCtorParam(nameof(MechanicAcceptanceReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.Driver.FirstName} {e.CheckPoint.Driver.LastName}"))
            .ForCtorParam(nameof(MechanicAcceptanceReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.MechanicId))
            .ForCtorParam(nameof(MechanicAcceptanceReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Mechanic.FirstName} {e.Mechanic.LastName}"));
    }
}
