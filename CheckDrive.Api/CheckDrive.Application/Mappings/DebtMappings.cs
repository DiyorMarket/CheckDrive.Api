using AutoMapper;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public sealed class DebtMappings : Profile
{
    public DebtMappings()
    {
        CreateMap<Debt, DebtDto>()
            .ForCtorParam(nameof(DebtDto.Id), cfg => cfg.MapFrom(e =>
            e.Id))
            .ForCtorParam(nameof(DebtDto.FirsName), cfg => cfg.MapFrom(e =>
            e.CheckPoint.DoctorReview.Driver.FirstName))
            .ForCtorParam(nameof(DebtDto.LastName), cfg => cfg.MapFrom(e =>
            e.CheckPoint.DoctorReview.Driver.LastName))
            .ForCtorParam(nameof(DebtDto.FualAmount), cfg => cfg.MapFrom(e =>
            e.FuelAmount))
            .ForCtorParam(nameof(DebtDto.PaidAmount), cfg => cfg.MapFrom(e =>
            e.PaidAmount))
            .ForCtorParam(nameof(DebtDto.Status), cfg => cfg.MapFrom(e =>
            e.Status));
    }
}
