using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using src.Models;
using src.Models.Dtos;
using System.Security.Claims;

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
            CreateMap<List<Claim>, UserResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                int.Parse(src.FirstOrDefault(c => c.Type == "userId")!.Value)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                src.FirstOrDefault(c => c.Type == "name")!.Value))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src =>
                src.FirstOrDefault(c => c.Type == "email")!.Value))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                src.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value))
                .ForMember(dest => dest.Money, opt => opt
                .MapFrom(src => MapMoneyClaim(src)));
        }
        private decimal MapMoneyClaim(List<Claim> claims)
        {
            var moneyClaim = claims.FirstOrDefault(c => c.Type == "money")?.Value;
            decimal money;

            if (decimal.TryParse(moneyClaim, out money))
            {
                return money;
            }
            else
            {
                return 0m; // Default value if the claim is not found or cannot be parsed
            }
        }
    }
}
