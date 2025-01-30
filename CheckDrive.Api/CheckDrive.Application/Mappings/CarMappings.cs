using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public class CarMappings : Profile
{
    public CarMappings()
    {
        CreateMap<Car, CarDto>()
            .ForMember(dest => dest.OilMarkId, opt => opt.MapFrom(src => src.OilMark!.Id))
            .ForMember(dest => dest.CurrentMonthMileage, opt => opt.MapFrom(src => src.UsageSummary.CurrentMonthDistance))
            .ForMember(dest => dest.CurrentYearMileage, opt => opt.MapFrom(src => src.UsageSummary.CurrentYearDistance))
            .ForMember(dest => dest.MonthlyDistanceLimit, opt => opt.MapFrom(src => src.Limits.MonthlyDistanceLimit))
            .ForMember(dest => dest.YearlyDistanceLimit, opt => opt.MapFrom(src => src.Limits.YearlyDistanceLimit))
            .ForMember(dest => dest.CurrentMonthFuelConsumption, opt => opt.MapFrom(src => src.UsageSummary.CurrentMonthFuelConsumption))
            .ForMember(dest => dest.CurrentYearFuelConsumption, opt => opt.MapFrom(src => src.UsageSummary.CurrentYearFuelConsumption))
            .ForMember(dest => dest.MonthlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.Limits.MonthlyFuelConsumptionLimit))
            .ForMember(dest => dest.YearlyFuelConsumptionLimit, opt => opt.MapFrom(src => src.Limits.YearlyFuelConsumptionLimit));

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