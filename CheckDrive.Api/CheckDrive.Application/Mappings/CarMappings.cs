using AutoMapper;
using CheckDrive.Application.DTOs.Car;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public sealed class CarMappings : Profile
{
    public CarMappings()
    {
        CreateMap<Car, CarDto>();
        CreateMap<CreateCarDto, Car>();
        CreateMap<UpdateCarDto, Car>();
    }
}
