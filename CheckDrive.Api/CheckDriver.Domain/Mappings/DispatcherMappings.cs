using AutoMapper;
using CheckDriver.Domain.Entities;
using CheckDrive.DTOs.Dispatcher;

namespace CheckDrive.Domain.Mappings
{
    public class DispatcherMappings : Profile
    {
        public DispatcherMappings()
        {
            CreateMap<DispatcherDto, Dispatcher>();
            CreateMap<Dispatcher, DispatcherDto>();
            CreateMap<DispatcherForCreateDto, Dispatcher>();
            CreateMap<DispatcherForUpdateDto, Dispatcher>();
        }
    }
}
