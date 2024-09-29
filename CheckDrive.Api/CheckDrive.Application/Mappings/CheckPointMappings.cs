using AutoMapper;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class CheckPointMappings : Profile
{
    public CheckPointMappings()
    {
        CreateMap<CheckPoint, CheckPointDto>();
    }
}
