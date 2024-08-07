﻿using AutoMapper;
using CheckDrive.ApiContracts.Account;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>()
                .ForMember(x => x.RoleName, e => e.MapFrom(d => d.Role.Name));
            CreateMap<AccountForCreateDto, Account>();
            CreateMap<AccountForCreateDto, Driver>();
            CreateMap<AccountForUpdateDto, Account>();
        }
    }
}
