using AutoMapper;
using CheckDrive.Application.DTOs.CheckPoint;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

internal sealed class CheckPointMappings : Profile
{
    public CheckPointMappings()
    {
        CreateMap<CheckPoint, CheckPointDto>()
            .ForCtorParam(nameof(CheckPointDto.Driver), cfg => cfg.MapFrom(e => e.DoctorReview.Driver))
            .ForCtorParam(nameof(CheckPointDto.Car), cfg => cfg.MapFrom(e => e.MechanicHandover.Car));
    }
}
