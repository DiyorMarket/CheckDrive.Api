using AutoMapper;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class CheckPointMappings : Profile
{
    public CheckPointMappings()
    {
        CreateMap<CheckPoint, CheckPointDto>()
            .ForCtorParam(nameof(CheckPointDto.Driver), opt => opt.MapFrom(src => $"{src.DoctorReview.Driver.FirstName} {src.DoctorReview.Driver.LastName}"))
            .ForCtorParam(nameof(CheckPointDto.CarModel), opt => opt.MapFrom(src => $"{src.MechanicHandover!.Car.Model}"))
            .ForCtorParam(nameof(CheckPointDto.CurrentFuelAmount), opt => opt.MapFrom(src => src.OperatorReview!.InitialOilAmount))
            .ForCtorParam(nameof(CheckPointDto.Mechanic), opt => opt.MapFrom(src => $"{src.MechanicHandover!.Mechanic.FirstName} {src.MechanicHandover.Mechanic.LastName}"))
            .ForCtorParam(nameof(CheckPointDto.InitialMillage), opt => opt.MapFrom(src => src.MechanicHandover!.InitialMileage))
            .ForCtorParam(nameof(CheckPointDto.FinalMileage), opt => opt.MapFrom(src => src.DispatcherReview!.FinalMileage))
            .ForCtorParam(nameof(CheckPointDto.Operator), opt => opt.MapFrom(src => $"{src.OperatorReview!.Operator.FirstName} {src.OperatorReview.Operator.LastName}"))
            .ForCtorParam(nameof(CheckPointDto.InitialOilAmount), opt => opt.MapFrom(src => src.OperatorReview!.InitialOilAmount))
            .ForCtorParam(nameof(CheckPointDto.OilRefillAmount), opt => opt.MapFrom(src => src.OperatorReview!.OilRefillAmount))
            .ForCtorParam(nameof(CheckPointDto.Oil), opt => opt.MapFrom(src => src.MechanicHandover!.Car.OilMark!.Name))
            .ForCtorParam(nameof(CheckPointDto.Dispatcher), opt => opt.MapFrom(src => $"{src.DispatcherReview!.Dispatcher.FirstName} {src.DispatcherReview.Dispatcher.LastName}"))
            .ForCtorParam(nameof(CheckPointDto.FuelConsumptionAdjustment), opt => opt.MapFrom(src => src.DispatcherReview!.FuelConsumptionAmount))
            .ForCtorParam(nameof(CheckPointDto.DebtAmount), opt => opt.MapFrom(src => src.ManagerReview!.DebtAmount));
    }
}
