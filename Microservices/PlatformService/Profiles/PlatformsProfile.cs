using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles{
    public class PlatformsProfile : Profile {
        public PlatformsProfile()
        {
            //Source to Target as prop name in Models and DTOs are same hence we need not to tell automapper anything else we might need to map explicitly 
            CreateMap<Platform, PlatformReadDto>(); // When user is reading platfroms
            CreateMap<PlatformCreateDto, Platform>(); // When user is creating platfroms
            CreateMap<PlatformReadDto,PlatformPublishedDto>();
        }
    }
}