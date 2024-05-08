using Castle.Core.Logging;
using FastFoodAPI.Utility;
using FastFoodHouse_API.Controllers;
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
using Xunit;

namespace FastFoodHouse_API.UnitTests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly OrderController _controller;
        private readonly Mock<IOrderService> _orderServiceMock = new Mock<IOrderService>();
        private readonly Mock<ICustomerService> _customerServiceMock = new Mock<ICustomerService>();

        public OrderControllerTests()
        {
            _controller = new OrderController(_orderServiceMock.Object, _customerServiceMock.Object);
        }

        [Fact]
        public async Task CreateOrder_ReturnsOk_WhenOrderCreatedSuccessfully()
        {
            // Arrange
            var userId = "123";
            var orderHeaderDTO = new OrderHeaderDTO { ApplicationUserId = userId };

            var userClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = userPrincipal;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;

            _orderServiceMock.Setup(x => x.CreateOrder(orderHeaderDTO)).ReturnsAsync(new OrderHeaderDTO());

            // Act
            var result = await _controller.CreateOrder(orderHeaderDTO);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Order created successfully.", actionResult.Value);
        }



        [Fact]
        public async Task GetOrders_ReturnsOk_WithOrderHeaders()
        {
            // Arrange
            var orderHeaders = new List<OrderHeaderDTO>
            {
                new OrderHeaderDTO { Id = 1, OrderTotal = 50.0 },
                new OrderHeaderDTO { Id = 2, OrderTotal = 60.0 },
                new OrderHeaderDTO { Id = 3, OrderTotal = 70.0 }
            };

            _orderServiceMock.Setup(x => x.GetOrders()).ReturnsAsync(orderHeaders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var actionResult = Assert.IsType<ActionResult<OrderHeaderDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var orderHeadersResult = Assert.IsType<List<OrderHeaderDTO>>(okResult.Value);
            Assert.Equal(orderHeaders.Count, orderHeadersResult.Count);
            Assert.Equal(orderHeaders[0].Id, orderHeadersResult[0].Id);
            Assert.Equal(orderHeaders[1].Id, orderHeadersResult[1].Id);
            Assert.Equal(orderHeaders[2].Id, orderHeadersResult[2].Id);
        }

    }
}
