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
            CreateMap<VillaCreateDTO, Villa>();
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>();
            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>();
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
