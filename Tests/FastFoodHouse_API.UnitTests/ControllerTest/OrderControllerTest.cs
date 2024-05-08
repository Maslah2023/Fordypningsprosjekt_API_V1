using FastFoodHouse_API.Controllers;
using FastFoodHouse_API.Service.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodHouse_API.UniTests.ControllerTests
{
    public class OrderControllerTest
    {
        private readonly OrderController _mockcontroller;
        private readonly Mock<IOrderService> _mockService = new Mock<IOrderService>();
        private readonly ICustomerService _customerSeervice;
        public OrderControllerTest()
        {
            _mockcontroller = new OrderController(_mockService.Object, _customerSeervice);
        }
        [Fact]

        public async Task Create_Orderheader_Retun_OrderDto()
        {

        }
    }
}
