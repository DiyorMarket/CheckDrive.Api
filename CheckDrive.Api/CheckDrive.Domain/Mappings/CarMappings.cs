﻿using AutoMapper;
using CheckDrive.ApiContracts.Car;
using CheckDrive.Domain.Entities;

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
