using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Mapper
{
    public class CartItemMapper : Profile
    {
        public CartItemMapper() 
        {
            CreateMap<CartItemCreateDTO, CartItem>();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<CartItem, MenuItem>().ReverseMap();   
        }
    }
}
