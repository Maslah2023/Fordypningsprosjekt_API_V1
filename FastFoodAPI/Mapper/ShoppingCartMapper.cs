using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Mapper
{
    public class ShoppingCartMapper : Profile
    {
     
            public ShoppingCartMapper()
            {
                CreateMap<ShoppingCart, CartItem>().ReverseMap();
                CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
                CreateMap<ShoppingCart, ShoppingCartCreateDTO>().ReverseMap();
            }
        
    }
}
