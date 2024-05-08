using FastFoodAPI.Utility;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerSeervice;

        public OrderController(IOrderService orderService, ICustomerService customerSeervice)
        {
            _orderService = orderService;
            _customerSeervice = customerSeervice;
        }


        [Authorize(Roles = SD.Role_Customer)]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderHeaderDTO orderHeaderDTO)
        {
            try
            {

                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    // Check if the current user is the same as the specified id or is an admin
                    if (currentUserId != orderHeaderDTO.ApplicationUserId)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }
                var createdOrder = await _orderService.CreateOrder(orderHeaderDTO);
                return Ok("Order created successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }


        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult<OrderHeaderDTO>> GetOrders()
        {
            try
            {

                IEnumerable<OrderHeaderDTO> orderHeaders = await _orderService.GetOrders();

                return Ok(orderHeaders);
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }


        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetorderById(int id)
        {
            try
            {

                if (id == 0)
                {
                    return BadRequest("Invalid ID");
                }

                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    // Check if the current user is the same as the specified id or is an admin
                    if (role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
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


        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrderHeaderDTO>> UpdateOrderHeader(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    if (string.IsNullOrEmpty(orderHeaderUpdateDTO.ApplicationUserId) && role != SD.Role_Admin)
                    {
                        return BadRequest("ApplicationUser Id must be provided");
                    }

                    // Check if the current user is the same as the specified id or is an admin
                    if (currentUserId != orderHeaderUpdateDTO.ApplicationUserId && role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }
               _orderService.UpdateOrder(id, orderHeaderUpdateDTO);
               return Ok("Order updated successfully.");
        
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Internal Server Error");
            }
        }



        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpDelete]
        public async Task<ActionResult<OrderHeaderDTO>> DeleteOrderById(int id, int menuId)
        {
            try
            {

                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                OrderHeaderDTO orderHeaderDTO = await _orderService.GetOrderById(id);

                if (roleClaim != null)
                {
                  
                    var role = roleClaim.Value;

           
                    if (currentUserId != orderHeaderDTO.ApplicationUserId && role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }

                _orderService.DeleteOrderById(id, menuId);
                return Ok("Order deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }
    }
}
