using AutoMapper;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class DebtMappings : Profile
{
    public DebtMappings()
    {
        CreateMap<Debt, DebtDto>()
           .ForCtorParam(nameof(DebtDto.Id), cfg => cfg.MapFrom(e => e.Id))
           .ForCtorParam(nameof(DebtDto.DriverFullName), cfg => cfg.MapFrom(e => e.CheckPoint.DoctorReview.Driver.LastName + " " + e.CheckPoint.DoctorReview.Driver.FirstName))
           .ForCtorParam(nameof(DebtDto.FuelAmount), cfg => cfg.MapFrom(e => e.FuelAmount))
           .ForCtorParam(nameof(DebtDto.PaidAmount), cfg => cfg.MapFrom(e => e.PaidAmount))
           .ForCtorParam(nameof(DebtDto.Status), cfg => cfg.MapFrom(e => e.Status))
           .ForCtorParam(nameof(DebtDto.CheckPointId), cfg => cfg.MapFrom(e => e.CheckPointId));

        CreateMap<DebtDto, Debt>()
           .ForMember(dest => dest.Id, cfg => cfg.MapFrom(e => e.Id))
           .ForMember(dest => dest.FuelAmount, cfg => cfg.MapFrom(e => e.FuelAmount))
           .ForMember(dest => dest.PaidAmount, cfg => cfg.MapFrom(e => e.PaidAmount))
           .ForMember(dest => dest.Status, cfg => cfg.MapFrom(e => e.Status))
           .ForMember(dest => dest.CheckPointId, cfg => cfg.MapFrom(e => e.CheckPointId));
    }
}
