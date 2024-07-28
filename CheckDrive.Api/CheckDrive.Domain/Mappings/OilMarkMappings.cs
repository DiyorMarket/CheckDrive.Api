using AutoMapper;
using CheckDrive.ApiContracts.OilMark;
using CheckDrive.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class OilMarkMappings : Profile
    {
        public OilMarkMappings()
        {
            CreateMap<OilMarkDto, OilMarks>();
            CreateMap<OilMarks, OilMarkDto>();
            CreateMap<OilMarkForCreateDto, OilMarks>();
            CreateMap<OilMarkForUpdateDto, OilMarks>();
        }
    }
}
