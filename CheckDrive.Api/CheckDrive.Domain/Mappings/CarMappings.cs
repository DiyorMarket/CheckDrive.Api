using AutoMapper;
using CheckDrive.ApiContracts.Car;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class CarMappings : Profile
    {
        public CarMappings()
        {
            CreateMap<CarDto, Car>();
            CreateMap<Car, CarDto>()
                .ForMember(x => x.Status, e => e.MapFrom(f => f.CarStatus))
                .ForMember(x => x.OneMonthMediumDistance, e => e.MapFrom(f => f.OneYearMediumDistance / 12))
                .ForMember(x => x.OneYearMeduimFuelConsumption, e => e.MapFrom(f => f.OneYearMediumDistance * (f.MeduimFuelConsumption / 100)))
                .ForMember(x => x.OneMonthMeduimFuelConsumption, e => e.MapFrom(f => (f.OneYearMediumDistance * (f.MeduimFuelConsumption / 100)) / 12));
            CreateMap<CarForCreateDto, Car>();
            CreateMap<CarForUpdateDto, Car>();
        }
    }
}