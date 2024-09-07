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
            CreateMap<Debts, DebtsDto>()
                .ForMember(x => x.DriverName, e => e.MapFrom(d => d.Driver.Account.FirstName + d.Driver.Account.LastName))
                .ForMember(x => x.CarName, e => e.MapFrom(d => d.Car.Model + d.Car.Number));
            CreateMap<DebtsForCreateDto, Debts>();
            CreateMap<DebtsForUpdateDto, Debts>();
        }
    }
}
