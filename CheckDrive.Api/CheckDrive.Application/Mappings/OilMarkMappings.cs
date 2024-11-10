using AutoMapper;
using CheckDrive.Application.DTOs.OilMark;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public sealed class OilMarkMappings : Profile
{
    public OilMarkMappings()
    {
        CreateMap<OilMark, OilMarkDto>();
        CreateMap<CreateOilMarkDto, OilMark>();
        CreateMap<UpdateOilMarkDto, OilMark>();
    }
}
