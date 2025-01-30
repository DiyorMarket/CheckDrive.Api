using AutoMapper;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Application.DTOs.Debt;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class CheckPointMappings : Profile
{

    public CheckPointMappings()
    {
        CreateMap<Debt, DebtDto>();

        CreateMap<CheckPoint, CheckPointDto>()
        .ForMember(dest => dest.Driver, cfg => cfg.MapFrom(src => src.DoctorReview.Driver))
        .ForMember(dest => dest.Car, cfg => cfg.MapFrom(src => src.MechanicHandover.Car))
        .ForMember(dest => dest.Debt, cfg => cfg.MapFrom(src => src.Debt))
        .ForMember(dest => dest.DoctorReview, cfg => cfg.MapFrom(src => src.DoctorReview))
        .ForMember(dest => dest.MechanicHandover, cfg => cfg.MapFrom(src => src.MechanicHandover))
        .ForMember(dest => dest.OperatorReview, cfg => cfg.MapFrom(src => src.OperatorReview))
        .ForMember(dest => dest.MechanicAcceptance, cfg => cfg.MapFrom(src => src.MechanicAcceptance))
        .ForMember(dest => dest.DispatcherReview, cfg => cfg.MapFrom(src => src.DispatcherReview))
        .ForMember(dest => dest.ManagerReview, cfg => cfg.MapFrom(src => src.ManagerReview));
    }
}
