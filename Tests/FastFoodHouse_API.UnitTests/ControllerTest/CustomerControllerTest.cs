using Castle.Core.Logging;
using FastFoodHouse_API.Controller;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.UniTests.ControllerTests
{
    public class CustomerControllerTest
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerService> _customerServiceMock = new Mock<ICustomerService>();
        private readonly ILogger<CustomerController> _logger;
        private readonly ICartItemService _cartItemService;



        public CustomerControllerTest()
        {
            _controller = new CustomerController(_customerServiceMock.Object, _logger, _cartItemService);
        }

        [Fact]

        public async Task GetCustomerByIdAsync_ReturnsOk_WhenCalledWithValidData()
        {
            var id = "1";
            var customerDTO = new CustomerDTO()
            {
                Id = id,
                Name = "Mslx",
                PhoneNumber = "1234567890",
                City = "test",
                Email = "test",
                Address = "test"



            };

            _customerServiceMock.Setup(u => u.GetCustomerById(id)).ReturnsAsync(customerDTO);

            var res = await _controller.GetUserById(id);

            var actionResult = Assert.IsType<ActionResult<CustomerDTO>>(res);
            var value = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<CustomerDTO>(value.Value);
            Assert.Equal(dto.Id, customerDTO.Id);
            Assert.Equal(dto.Name, customerDTO.Name);
            Assert.Equal(dto.PhoneNumber, customerDTO.PhoneNumber);
            Assert.Equal(dto.Email, customerDTO.Email);
            Assert.Equal(dto.City, customerDTO.City);




        }


        [Fact]
        public async Task GetCustomer_ShouldReturn_CustomerDTO()
        {
            int count = 0;
            // Arrange
            List<CustomerDTO> customerDTO = new()
            {
                 new CustomerDTO()
                 {
                   Id = "1",
                   Name = "Mslx",
                   PhoneNumber = "1234567890",
                   City = "test",
                   Email = "test",
                   Address = "test"
                 },
                  new CustomerDTO()
                 {
                   Id = "2",
                   Name = "Ahmed",
                   PhoneNumber = "1234567890",
                   City = "test",
                   Email = "test",
                   Address = "test"

                 },
                   new CustomerDTO()
                 {
                   Id = "3",
                   Name = "Usama",
                   PhoneNumber = "1234567890",
                   City = "test",
                   Email = "test",
                   Address = "test"

                 }



            };




            _customerServiceMock.Setup(x => x.GetAllCustomers()).ReturnsAsync(customerDTO);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<ActionResult<CustomerDTO>>(result);
            var returneValue = Assert.IsType<OkObjectResult>(okResult.Result);
            var dto_Result = Assert.IsType<List<CustomerDTO>>(returneValue.Value);
            Assert.Equal(3, customerDTO.Count());
            var dto = customerDTO.FirstOrDefault();


            foreach (var customerDto in dto_Result)
            {
                Assert.Equal(customerDto.Id, customerDTO[count].Id);
                Assert.Equal(customerDto.Name, customerDTO[count].Name);
                count++;
            }




        }




    }
}
