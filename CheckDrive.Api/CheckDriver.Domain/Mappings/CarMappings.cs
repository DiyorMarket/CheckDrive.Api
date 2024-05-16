using AutoMapper;
using CheckDriver.Domain.Entities;
using CheckDrive.DTOs.Car;

namespace CheckDrive.Domain.Mappings
{
    public class CarMappings : Profile
    {
        public CarMappings()
        {
            CreateMap<CarDto, Car>();
            CreateMap<Car, CarDto>();
            CreateMap<CarForCreateDto, Car>();
            CreateMap<CarForUpdateDto, Car>();
        }
    }
}
