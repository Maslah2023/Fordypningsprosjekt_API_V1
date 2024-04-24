using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Mapper
{
    public class MenuMapper : Profile
    {
        public MenuMapper()
        {
            CreateMap<MenuItem, MenuDTO>().ReverseMap();    
        }
    }
}
