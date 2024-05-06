using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FastFoodHouse_API.Controller
{
    [Route("api/customer")]
    [ApiController]

    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerService> _logger;
        protected ApiResponse _apiResponse;
        private readonly ICartItemService _cartItemService;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, ICartItemService cartItemService)
        {
            _customerService = customerService;
            _apiResponse = new ApiResponse();
            _cartItemService = cartItemService;
        }






        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAllUsers(string userId)
        {
            try
            {
                var users = await _customerService.GetAllCustomers();
                if (users == null)
                {
                    return NotFound();
                }
                _apiResponse.Result = users;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while retrieving customers: {ex.Message}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Message = "Internal Server Error";
                return _apiResponse;

            }



        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {

        
            //// Retrieve the currently logged-in user's ID from claims
            //var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (currentUser != id)
            //{
            //    return Unauthorized();
            //}
            var loginUser = await _customerService.GetCustomerById(id);
            if (loginUser == null)
            {
                return NotFound();
            }

            return Ok(loginUser);
        }


        //[Authorize(Roles = SD.Role_Admin  + "," + SD.Role_Customer)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomerAsync(string id, UpdateCustomerDTO updateCustomerDTO, string currentPassword, string newPassword)
        {
            try
            {
                //string loggedInUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if(loggedInUser != id || loggedInUser == null) 
                //{
                //    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //    _apiResponse.IsSuccess = false;
                //    return BadRequest(_apiResponse);
                //}
                var customer = await _customerService.UpdateCustomer(id, updateCustomerDTO, currentPassword, newPassword);
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError($"An error occured while retrieving customers: {ex.Message}");
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.Message = "Internal Server Error";
                return _apiResponse;

            }
           
        }



        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteCustomer(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }
                Customer deletedCustomer =  await _customerService.DeleteCustomer(userId);
                return Ok(deletedCustomer);

            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);

            }
            
         

        }


    }
}
