using AutoMapper;
using CheckDrive.ApiContracts.Debts;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DebtsMappings : Profile
    {
        public DebtsMappings() 
        {
            CreateMap<DebtsDto, Debts>();
            CreateMap<Debts, DebtsDto>();
            CreateMap<DebtsForCreateDto, Debts>();
            CreateMap<DebtsForUpdateDto, Debts>();
        }
    }
}
