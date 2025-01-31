using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public class CarMappings : Profile
{
    public CarMappings()
    {
        CreateMap<Car, CarDto>()
            .ForCtorParam(nameof(CarDto.Id), cfg => cfg.MapFrom(c => c.Id))
            .ForCtorParam(nameof(CarDto.OilMarkId), cfg => cfg.MapFrom(c => c.OilMarkId))
            .ForCtorParam(nameof(CarDto.Model), cfg => cfg.MapFrom(c => c.Model))
            .ForCtorParam(nameof(CarDto.Number), cfg => cfg.MapFrom(c => c.Number))
            .ForCtorParam(nameof(CarDto.ManufacturedYear), cfg => cfg.MapFrom(c => c.ManufacturedYear))
            .ForCtorParam(nameof(CarDto.Mileage), cfg => cfg.MapFrom(c => c.Mileage))
            .ForCtorParam(nameof(CarDto.CurrentMonthFuelConsumption), cfg => cfg.MapFrom(c => c.UsageSummary.CurrentMonthFuelConsumption))
            .ForCtorParam(nameof(CarDto.CurrentYearFuelConsumption), cfg => cfg.MapFrom(c => c.UsageSummary.CurrentYearFuelConsumption))
            .ForCtorParam(nameof(CarDto.MonthlyFuelConsumptionLimit), cfg => cfg.MapFrom(c => c.Limits.MonthlyFuelConsumptionLimit))
            .ForCtorParam(nameof(CarDto.YearlyFuelConsumptionLimit), cfg => cfg.MapFrom(c => c.Limits.YearlyFuelConsumptionLimit))
            .ForCtorParam(nameof(CarDto.CurrentMonthMileage), cfg => cfg.MapFrom(c => c.UsageSummary.CurrentMonthDistance))
            .ForCtorParam(nameof(CarDto.CurrentYearMileage), cfg => cfg.MapFrom(c => c.UsageSummary.CurrentYearDistance))
            .ForCtorParam(nameof(CarDto.MonthlyDistanceLimit), cfg => cfg.MapFrom(c => c.Limits.MonthlyDistanceLimit))
            .ForCtorParam(nameof(CarDto.YearlyDistanceLimit), cfg => cfg.MapFrom(c => c.Limits.YearlyDistanceLimit));

        CreateMap<CreateCarDto, Car>()
            .ForPath(dest => dest.OilMarkId, opt => opt.MapFrom(src => src.OilMarkId))
            .ForPath(dest => dest.UsageSummary.CurrentMonthDistance, opt => opt.MapFrom(src => src.CurrentYearMileage))
            .ForPath(dest => dest.UsageSummary.CurrentYearDistance, opt => opt.MapFrom(src => src.CurrentYearMileage))
            .ForPath(dest => dest.Limits.MonthlyDistanceLimit, opt => opt.MapFrom(src => src.MonthlyDistanceLimit))
            .ForPath(dest => dest.Limits.YearlyDistanceLimit, opt => opt.MapFrom(src => src.YearlyDistanceLimit))
            .ForPath(dest => dest.UsageSummary.CurrentMonthFuelConsumption, opt => opt.MapFrom(src => src.CurrentMonthFuelConsumption))
            .ForPath(dest => dest.UsageSummary.CurrentYearFuelConsumption, opt => opt.MapFrom(src => src.CurrentYearFuelConsumption))
            .ForPath(dest => dest.Limits.MonthlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.MonthlyFuelConsumptionLimit))
            .ForPath(dest => dest.Limits.YearlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.YearlyFuelConsumptionLimit));

        CreateMap<UpdateCarDto, Car>()
            .ForPath(dest => dest.OilMarkId, opt => opt.MapFrom(src => src.OilMarkId))
            .ForPath(dest => dest.UsageSummary.CurrentMonthDistance, opt => opt.MapFrom(src => src.CurrentYearMileage))
            .ForPath(dest => dest.UsageSummary.CurrentYearDistance, opt => opt.MapFrom(src => src.CurrentYearMileage))
            .ForPath(dest => dest.Limits.MonthlyDistanceLimit, opt => opt.MapFrom(src => src.MonthlyDistanceLimit))
            .ForPath(dest => dest.Limits.YearlyDistanceLimit, opt => opt.MapFrom(src => src.YearlyDistanceLimit))
            .ForPath(dest => dest.UsageSummary.CurrentMonthFuelConsumption, opt => opt.MapFrom(src => src.CurrentMonthFuelConsumption))
            .ForPath(dest => dest.UsageSummary.CurrentYearFuelConsumption, opt => opt.MapFrom(src => src.CurrentYearFuelConsumption))
            .ForPath(dest => dest.Limits.MonthlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.MonthlyFuelConsumptionLimit))
            .ForPath(dest => dest.Limits.YearlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.YearlyFuelConsumptionLimit));
    }
}