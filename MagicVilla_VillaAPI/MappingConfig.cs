﻿using AutoMapper;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI
{
    public class MappingConfig : Profile 
    {
        public MappingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
            
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
