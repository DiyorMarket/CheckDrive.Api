using AutoMapper;
using CheckDriver.Domain.Entities;
using CheckDrive.DTOs.Account;

namespace CheckDrive.Domain.Mappings
{
    public class AccountMappings : Profile
    {
        public AccountMappings() 
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDto>();
            CreateMap<AccountForCreateDto, Account>();
            CreateMap<AccountForUpdateDto, Account>();
        }
    }
}
