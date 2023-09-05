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
            CreateMap<Item, ItemRequestDto>().ReverseMap();
            // CreateMap<Item, ItemResponseDto>()
            //     .ForMember(dest => dest.SellerName,
            //                opt => opt.MapFrom(src => src.Seller.Name));
            CreateMap<Bid, BidResponseDto>()
                .ForMember(dest => dest.BiderName, opt => opt.MapFrom(src => src.Bider!.Name));

            CreateMap<Item, ItemResponseDto>()
                .ForMember(dest => dest.SellerName, opt => opt.MapFrom(src => src.Seller!.Name))
                .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids)); // Map the Bids property

            CreateMap<Bid, BidRequestDto>().ReverseMap();
        }
    }
}
