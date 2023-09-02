using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using src.Models;
using src.Models.Dtos;

namespace src.helpers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserResponseDto, User>().ReverseMap();
            CreateMap<Item, ItemResponseDto>().ReverseMap();
        }
    }
}
