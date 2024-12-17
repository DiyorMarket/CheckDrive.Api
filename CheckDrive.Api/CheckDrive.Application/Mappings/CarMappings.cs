using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public class CarMappings : Profile
{
    public CarMappings()
    {
        CreateMap<Car, CarDto>()
            .ForCtorParam(nameof(CarDto.CurrentMonthMileage), opt => opt.MapFrom(src => src.UsageSummary.CurrentMonthDistance))
            .ForCtorParam(nameof(CarDto.CurrentYearMileage), opt => opt.MapFrom(src => src.UsageSummary.CurrentYearDistance))
            .ForCtorParam(nameof(CarDto.MonthlyDistanceLimit), opt => opt.MapFrom(src => src.Limits.MonthlyDistanceLimit))
            .ForCtorParam(nameof(CarDto.YearlyDistanceLimit), opt => opt.MapFrom(src => src.Limits.YearlyDistanceLimit))
            .ForCtorParam(nameof(CarDto.CurrentMonthFuelConsumption), opt => opt.MapFrom(src => src.UsageSummary.CurrentMonthFuelConsumption))
            .ForCtorParam(nameof(CarDto.CurrentYearFuelConsumption), opt => opt.MapFrom(src => src.UsageSummary.CurrentYearFuelConsumption))
            .ForCtorParam(nameof(CarDto.MonthlyFuelConsumptionLimit), opt => opt.MapFrom(src => src.Limits.MonthlyFuelConsumptionLimit))
            .ForCtorParam(nameof(CarDto.YearlyFuelConsumptionLimit), opt => opt.MapFrom(src => src.Limits.YearlyFuelConsumptionLimit));
        CreateMap<CreateCarDto, Car>();
        CreateMap<UpdateCarDto, Car>();
    }
}
