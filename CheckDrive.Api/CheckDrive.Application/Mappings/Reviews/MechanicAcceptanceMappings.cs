using AutoMapper;

namespace CheckDrive.Application.Mappings.Reviews;

internal sealed class MechanicAcceptanceMappings : Profile
{
    public MechanicAcceptanceMappings()
    {
        //CreateMap<MechanicAcceptance, MechanicAcceptanceReviewDto>()
        //    .ForCtorParam(nameof(MechanicAcceptanceReviewDto.DriverId), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.DriverId))
        //    .ForCtorParam(nameof(MechanicAcceptanceReviewDto.DriverName), cfg => cfg.MapFrom(e => $"{e.CheckPoint.DoctorReview.Driver.FirstName} {e.CheckPoint.DoctorReview.Driver.LastName}"))
        //    .ForCtorParam(nameof(MechanicAcceptanceReviewDto.ReviewerId), cfg => cfg.MapFrom(e => e.MechanicId))
        //    .ForCtorParam(nameof(MechanicAcceptanceReviewDto.ReviewerName), cfg => cfg.MapFrom(e => $"{e.Mechanic.FirstName} {e.Mechanic.LastName}"));
    }
}
