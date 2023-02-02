using AutoMapper;
using Village_API.Models;
using Village_API.Models.Dto;

namespace Village_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillageDto>();
            CreateMap<VillageDto, Villa>();
            CreateMap<Villa, VillageCreateDto>().ReverseMap();
            CreateMap<Villa, VillageUpdateDto>().ReverseMap();
        }
    }
}
