using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
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


        //[HttpGet]

        //public async Task<IActionResult> GetOrders(string userId, string searchString, string status, int pageNumber = 1, int pageSize = 5)
        //{
        //    try
        //    {
        //        IEnumerable<OrderHeader> orderHeaders =
        //       _db.OrderHeaders.Include(_ => _.OrderDetails)
        //       .ThenInclude(_ => _.MenuItem)
        //       .OrderByDescending(u => u.OrderheaderId);

        //        if (!string.IsNullOrEmpty(userId))
        //        {
        //            orderHeaders = 
        //           _db.OrderHeaders
        //           .Where(u => u.ApplicationUserId == userId);
        //        }

        //        if(!string.IsNullOrEmpty(status))
        //        {
        //            orderHeaders = _db.OrderHeaders.Where(u => u.Status.ToLower() == status.ToLower());
        //        }


        //        if (!string.IsNullOrEmpty(searchString))
        //        {
        //            orderHeaders = _db.OrderHeaders.Where(u => u.PickUpPhoneNumber.ToLower() == searchString.ToLower()
        //            || u.PickupName.ToLower() == searchString.ToLower()
        //            || u.PickupEmail == searchString.ToLower());
        //        }

        //        Pagination pagination = new()
        //        {
        //            CurrentPage = pageNumber,
        //            PageSize = pageSize,
        //            TotalRecords = orderHeaders.Count()

        //        };

        //        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));

        //        _apiResponse.Result = orderHeaders;
        //        _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
        //        return Ok(_apiResponse);


        //    }
        //    catch(Exception ex) 
        //    {
        //        _apiResponse.ErrorMessages = new List<string> { ex.Message };
        //        _apiResponse.IsSuccess = false;
        //    }

        //    return BadRequest(_apiResponse);

        //}


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderHeaderDTO orderHeaderDTO)
        {
            try
            {
                //OrderHeader order = new OrderHeader()
                //{
                //    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                //    PickupName = orderHeaderDTO.PickupName,
                //    PickupEmail = orderHeaderDTO.PickupEmail,
                //    PickUpPhoneNumber = orderHeaderDTO.PickUpPhoneNumber,
                //    OrderDate = DateTime.Now,
                //    StripePaymentIntentID = orderHeaderDTO.StripePaymentIntentID,
                //    TotalItems = orderHeaderDTO.TotalItems,
                //    Status = string.IsNullOrEmpty(orderHeaderDTO.Status)? SD.status_pending : orderHeaderDTO.Status,
                //};

                //_db.OrderHeaders.Add(order);
                //await _db.SaveChangesAsync();
                //foreach (var orderDetailsDTO in orderHeaderDTO.OrderDetails)
                //{
                //    OrderDetailDTO orderDetail = new OrderDetailDTO()
                //    {
                //        MenuItemId = orderDetailsDTO.MenuItemId,
                //        ItemName = orderDetailsDTO.ItemName,
                //        Price = orderDetailsDTO.Price,
                //        OrderHeaderId = orderDetailsDTO.OrderHeaderId,
                //        Quantity = orderDetailsDTO.Quantity,
                //    };
                //    _db.orderDetails.Add(orderDetail);
                //}
                //await _db.SaveChangesAsync();
                //_apiResponse.Result = order;
                //order.OrderDetails = null;
                //_apiResponse.StatusCode = System.Net.HttpStatusCode.Created;
                //return Ok(_apiResponse);
                var createdOrder = _orderService.CreateOrder(orderHeaderDTO);

            } catch (Exception ex)

            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message };
            }
            return BadRequest(_apiResponse);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetOrders(string? userId)
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
                _apiResponse.ErrorMessages = new List<string> { ex.Message };

            }
            return _apiResponse;

        }


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
                OrderHeaderDTO? orderDTO = await
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

    }
}
