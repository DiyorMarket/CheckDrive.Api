using AutoMapper;
using CheckDrive.DTOs.DispatcherReview;
using CheckDriver.Domain.Entities;

namespace CheckDrive.Domain.Mappings
{
    public class DispatcherReviewMappings : Profile
    {
        public DispatcherReviewMappings() 
        {
            CreateMap<DispatcherReviewDto, DispatcherReview>();
            CreateMap<DispatcherReview, DispatcherReviewDto>();
            CreateMap<DispatcherReviewForCreateDto, DispatcherReview>();
            CreateMap<DispatcherReviewForUpdateDto, DispatcherReview>();
        }
    }
}
