using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Domain.Entities;
using AutoMapper;
using CheckDrive.ApiContracts.Role;

namespace CheckDrive.Domain.Mappings
{
    public class OilMarkMappings : Profile
    {
        public OilMarkMappings()
        {
            CreateMap<OilMarkDto, Role>();
            CreateMap<Role, OilMarkDto>();
            CreateMap<OilMarkForCreateDto, Role>();
            CreateMap<OilMarkForUpdateDto, Role>();
        }
    }
}
