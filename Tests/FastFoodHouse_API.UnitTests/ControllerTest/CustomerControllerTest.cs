using Castle.Core.Logging;
using FastFoodAPI.Utility;
using FastFoodHouse_API.Controller;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            // Arrange
            var userId = "123";
            var adminRoleClaim = new Claim(ClaimTypes.Role, SD.Role_Admin); // Assuming SD.Role_Admin is "Admin"
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            var userClaims = new List<Claim> { adminRoleClaim, userIdClaim };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = userPrincipal;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;







            var id = "1";
            var expectedCustomerDTO = new CustomerDTO
            {
                Id = id,
                Name = "Maslah",
                PhoneNumber = "1234567890",
                City = "test",
                Email = "test",
                Address = "test"
            };

            _customerServiceMock.Setup(u => u.GetCustomerById(id)).ReturnsAsync(expectedCustomerDTO);

            // Act
            var result = await _controller.GetUserById(id);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CustomerDTO>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var actualCustomerDTO = Assert.IsType<CustomerDTO>(okObjectResult.Value);

            Assert.Equal(expectedCustomerDTO.Id, actualCustomerDTO.Id);
            Assert.Equal(expectedCustomerDTO.Name, actualCustomerDTO.Name);
            Assert.Equal(expectedCustomerDTO.PhoneNumber, actualCustomerDTO.PhoneNumber);
            Assert.Equal(expectedCustomerDTO.Email, actualCustomerDTO.Email);
            Assert.Equal(expectedCustomerDTO.City, actualCustomerDTO.City);
        }



        [Fact]
        public async Task GetCustomerById_Returns_OkObjectResult_For_Admin()
        {
            // Arrange

            var userId = "123";
            var adminRoleClaim = new Claim(ClaimTypes.Role, SD.Role_Admin); // Assuming SD.Role_Admin is "Admin"
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            var userClaims = new List<Claim> { adminRoleClaim, userIdClaim };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = userPrincipal;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;


            var expectedCustomer = new CustomerDTO
            {
                Name = "Test",
                PhoneNumber = "1234567890",
                Address = "Test",
                City = "Test",
            };
            _customerServiceMock.Setup(c => c.GetCustomerById(userId)).ReturnsAsync(expectedCustomer);

            // Act
            var result = await _controller.GetUserById(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<CustomerDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var customer = Assert.IsType<CustomerDTO>(okResult.Value);
            Assert.Equal(expectedCustomer, customer);
        }


        [Fact]
        public async Task GetCustomers_ShouldReturn_ListofCustomers()
        {
            // Arrange
            // Arrange
            var userId = "123";
            var adminRoleClaim = new Claim(ClaimTypes.Role, SD.Role_Admin); // Assuming SD.Role_Admin is "Admin"
            var userIdClaim = new Claim(ClaimTypes.NameIdentifier, userId);
            var userClaims = new List<Claim> { adminRoleClaim, userIdClaim };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = userPrincipal;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;



            var customers = new List<CustomerDTO>
    {
        new CustomerDTO { Id = "1", Name = "Maslah", PhoneNumber = "1234567890", City = "test", Email = "test", Address = "test" },
        new CustomerDTO { Id = "2", Name = "Ahmed", PhoneNumber = "1234567890", City = "test", Email = "test", Address = "test" },
        new CustomerDTO { Id = "3", Name = "Ole Nordmann", PhoneNumber = "1234567890", City = "test", Email = "test", Address = "test" },
        new CustomerDTO { Id = "3", Name = "Fahad", PhoneNumber = "1234567890", City = "test", Email = "test", Address = "test" }
    };

            _customerServiceMock.Setup(x => x.GetAllCustomers()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<CustomerDTO>>(result);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtoResult = Assert.IsType<List<CustomerDTO>>(objectResult.Value);

            Assert.Equal(customers.Count, dtoResult.Count);

            // Assert each customer individually using index access
            Assert.Equal(customers[0].Id, dtoResult[0].Id);
            Assert.Equal(customers[0].Name, dtoResult[0].Name);

            Assert.Equal(customers[1].Id, dtoResult[1].Id);
            Assert.Equal(customers[1].Name, dtoResult[1].Name);

            Assert.Equal(customers[2].Id, dtoResult[2].Id);
            Assert.Equal(customers[2].Name, dtoResult[2].Name);
        }





    }




}

