using AutoMapper;
using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Mapper
{
    public class ShoppingCartMapper : Profile
    {
     
            public ShoppingCartMapper()
            {
                CreateMap<ShoppingCart, CartItem>().ReverseMap();
            }
        
    }
}
