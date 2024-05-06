using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, UserDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
        }
    }
}
