using AutoMapper;
using CheckDrive.Application.DTOs.Driver;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Application.Mappings;

public sealed class EmployeeMappings : Profile
{
    public EmployeeMappings()
    {
        CreateMap<Employee, DriverDto>()
            .ForMember(x => x.FullName, cfg => cfg.MapFrom(e => $"{e.FirstName} {e.LastName}"));
    }
}
