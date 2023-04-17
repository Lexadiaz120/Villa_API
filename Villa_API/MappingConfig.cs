using AutoMapper;
using System.Runtime;
using Villa_API.Models;
using Villa_API.Models.Dto;

namespace Villa_API
{
    public class MappingConfig : Profile
    { 
        public MappingConfig() {
            CreateMap<Villa, VillaDTO>(); 
            CreateMap<VillaDTO, Villa>();
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
        }
    }
}
