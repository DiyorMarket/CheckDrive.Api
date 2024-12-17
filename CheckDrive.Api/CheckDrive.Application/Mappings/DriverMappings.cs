using AutoMapper;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class DriverMappings : Profile
{
    public DriverMappings()
    {
        CreateMap<Driver, DriverDto>()
            .ForCtorParam(nameof(DriverDto.FullName), cfg => cfg.MapFrom(e => $"{e.FirstName} {e.Patronymic} {e.LastName}"));
    }
}
