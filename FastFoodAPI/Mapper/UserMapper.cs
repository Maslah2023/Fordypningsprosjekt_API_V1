using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Runtime;

namespace FastFoodHouse_API.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<Customer, UserDto>().ReverseMap();
        }
    }
}
