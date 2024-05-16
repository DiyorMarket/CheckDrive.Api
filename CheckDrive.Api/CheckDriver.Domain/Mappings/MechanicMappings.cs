using AutoMapper;
using CheckDrive.DTOs.Mechanic;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class MechanicMappings : Profile
    {
        public MechanicMappings() 
        {
            CreateMap<MechanicDto, Mechanic>();
            CreateMap<Mechanic, MechanicDto>();
            CreateMap<MechanicForCreateDto, Mechanic>();
            CreateMap<MechanicForUpdateDto, Mechanic>();
        }
    }
}
