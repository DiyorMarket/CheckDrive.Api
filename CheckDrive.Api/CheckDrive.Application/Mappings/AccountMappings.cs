using AutoMapper;
using CheckDrive.Application.DTOs.Employee;
using CheckDrive.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Mappings;

internal sealed class AccountMappings : Profile
{
    public AccountMappings()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForCtorParam(nameof(EmployeeDto.Id), cfg => cfg.MapFrom(e => e.Id))
            .ForCtorParam(nameof(EmployeeDto.AccountId), cfg => cfg.MapFrom(e => e.AccountId))
            .ForCtorParam(nameof(EmployeeDto.Username), cfg => cfg.MapFrom(e => e.Account.UserName))
            .ForCtorParam(nameof(EmployeeDto.PhoneNumber), cfg => cfg.MapFrom(e => e.Account.PhoneNumber))
            .ForCtorParam(nameof(EmployeeDto.Email), cfg => cfg.MapFrom(e => e.Account.Email))
            .ForCtorParam(nameof(EmployeeDto.FirstName), cfg => cfg.MapFrom(e => e.FirstName))
            .ForCtorParam(nameof(EmployeeDto.LastName), cfg => cfg.MapFrom(e => e.LastName))
            .ForCtorParam(nameof(EmployeeDto.Passport), cfg => cfg.MapFrom(e => e.Passport))
            .ForCtorParam(nameof(EmployeeDto.Address), cfg => cfg.MapFrom(e => e.Address))
            .ForCtorParam(nameof(EmployeeDto.Birthdate), cfg => cfg.MapFrom(e => e.Birthdate))
            .ForCtorParam(nameof(EmployeeDto.Position), cfg => cfg.MapFrom(e => e.Position));

        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<CreateEmployeeDto, IdentityUser>();

        CreateMap<UpdateEmployeeDto, UpdateEmployeeDto>();
        CreateMap<UpdateEmployeeDto, IdentityUser>();
    }
}
