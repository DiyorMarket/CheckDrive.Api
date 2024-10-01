using AutoMapper;
using CheckDrive.Application.DTOs.Account;
using CheckDrive.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CheckDrive.Application.Mappings;

internal sealed class AccountMappings : Profile
{
    public AccountMappings()
    {
        CreateMap<Employee, AccountDto>()
            .ForCtorParam(nameof(AccountDto.Id), cfg => cfg.MapFrom(e => e.AccountId))
            .ForCtorParam(nameof(AccountDto.Username), cfg => cfg.MapFrom(e => e.Account.UserName))
            .ForCtorParam(nameof(AccountDto.PhoneNumber), cfg => cfg.MapFrom(e => e.Account.PhoneNumber))
            .ForCtorParam(nameof(AccountDto.Email), cfg => cfg.MapFrom(e => e.Account.Email))
            .ForCtorParam(nameof(AccountDto.FirstName), cfg => cfg.MapFrom(e => e.FirstName))
            .ForCtorParam(nameof(AccountDto.LastName), cfg => cfg.MapFrom(e => e.LastName))
            .ForCtorParam(nameof(AccountDto.Passport), cfg => cfg.MapFrom(e => e.Passport))
            .ForCtorParam(nameof(AccountDto.Address), cfg => cfg.MapFrom(e => e.Address))
            .ForCtorParam(nameof(AccountDto.Birthdate), cfg => cfg.MapFrom(e => e.Birthdate))
            .ForCtorParam(nameof(AccountDto.Position), cfg => cfg.MapFrom(e => e.Position));

        CreateMap<CreateAccountDto, Employee>();
        CreateMap<CreateAccountDto, IdentityUser>();

        CreateMap<UpdateAccountDto, UpdateAccountDto>();
        CreateMap<UpdateAccountDto, IdentityUser>();
    }
}
