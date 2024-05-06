using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FastFoodHouse_API.Controller
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ApplicationDbContext _db;
        private ApiResponse _apiResponse;

        public OrderController(IOrderService orderService, ApplicationDbContext db)
        {
            _orderService = orderService;
            _apiResponse = new ApiResponse();
            _db = db;
        }


  

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderHeaderDTO orderHeaderDTO)
        {
            try
            {
               
                var createdOrder = await _orderService.CreateOrder(orderHeaderDTO);

            } catch (Exception ex)

            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = "Internal Server Error";
            }
            return BadRequest(_apiResponse);
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string userId)
        {
            try
            {
                IEnumerable<OrderHeaderDTO> orderHeaders = await
                _orderService.GetOrders(userId);
                if (!string.IsNullOrEmpty(userId))
                {
                    orderHeaders = orderHeaders.Where(u => u.ApplicationUserId == userId);
                }
                else
                {
                    _apiResponse.Result = orderHeaders;
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                }
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Message = "Internal Server Error";

            }
            return _apiResponse;

        }

      //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> GetorderById(int id)
        {
            try
            {
                if (id == 0)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest(_apiResponse);
                }
                OrderHeaderDTO orderDTO = await
                _orderService.GetOrderById(id);
                if (orderDTO == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return NotFound(_apiResponse);
                }
                _apiResponse.StatusCode= System.Net.HttpStatusCode.OK;
                _apiResponse.Result = orderDTO;
            }
            catch (Exception ex)
            {

                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return _apiResponse;
        }


        
        [HttpPut("{id:int}")]
        public ActionResult<ApiResponse> UpdateOrderHeader(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _orderService.UpdateOrder(id, orderHeaderUpdateDTO);
                }
                else
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                }
            }
            catch(Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return BadRequest(_apiResponse);
            }
            _apiResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            return NoContent();
           
         
        }

        [HttpDelete]
        public ActionResult DeleteOrderById(int id, int menuId)
        {
        
           _orderService.DeleteOrderById(id, menuId);
            return NoContent();
        }

    }
}
