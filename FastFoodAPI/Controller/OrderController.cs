using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderHeaderDTO orderHeaderDTO)
        {
            try
            {
                var createdOrder = await _orderService.CreateOrder(orderHeaderDTO);
                return Ok(createdOrder);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(string userId)
        {
            try
            {
                IEnumerable<OrderHeaderDTO> orderHeaders = await _orderService.GetOrders(userId);
                if (!string.IsNullOrEmpty(userId))
                {
                    orderHeaders = orderHeaders.Where(u => u.ApplicationUserId == userId);
                }
                return Ok(orderHeaders);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetorderById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid ID");
                }
                OrderHeaderDTO orderDTO = await _orderService.GetOrderById(id);
                if (orderDTO == null)
                {
                    return NotFound("Order not found");
                }
                return Ok(orderDTO);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOrderHeader(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _orderService.UpdateOrder(id, orderHeaderUpdateDTO);
                    return NoContent();
                }
                else
                {
                    return BadRequest("Invalid model state");
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }

        [HttpDelete]
        public IActionResult DeleteOrderById(int id, int menuId)
        {
            try
            {
                _orderService.DeleteOrderById(id, menuId);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }
    }
}
