using AutoMapper;
using FastFoodHouse_API.Mapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.UniTests.MapperTest
{
    public class CustomerMapperTest
    {
        private readonly IMapper _mapper;

        public CustomerMapperTest()
        {
            //// Setup AutoMapper using a configuration instance
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Customer, CustomerDTO>(); // Map from Customer to CustomerDTO
            //});

            //_mapper = config.CreateMapper();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerMapper()));
            _mapper = configuration.CreateMapper();

        }

       


    

        [Fact]
        public void MapToDto_WhenCustomer_Given_ShouldReturn_CustomerDto()
        {
            //Arrange
            Customer customer = new Customer()
            {
                Id = "4342DJE99D0S-SDSDDSDDSD-DSDSSDD-DSD",
                Name = "test",
                PhoneNumber = "test",
                Address = "test",
                City = "test",
            };

            var customerDto = _mapper.Map<CustomerDTO>(customer);
            Assert.Equal(customer.Name, customerDto.Name);
            Assert.Equal(customer.City, customerDto.City);
        }
    }
}
