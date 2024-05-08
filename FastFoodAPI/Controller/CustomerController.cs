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
        private readonly ICartItemService _cartItemService;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, ICartItemService cartItemService)
        {
            _customerService = customerService;
            _cartItemService = cartItemService;
        }






        [Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllCustomers();
                if (customers.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving customers: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }


        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetUserById(string id)
        {

            try
            {
                // Retrieve the currently logged-in user's ID from claims
                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    // Check if the current user is the same as the specified id or is an admin
                    if (currentUserId != id && role != SD.Role_Admin)
                    {
                        return Unauthorized("Access Denied");
                    }
                }
                var customer = await _customerService.GetCustomerById(id);
                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(customer);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Internal server error");
            }
        }


        [Authorize(Roles = SD.Role_Admin  + "," + SD.Role_Customer)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateCustomerAsync(string id, UpdateCustomerDTO updateCustomerDTO, string currentPassword, string newPassword)
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
                    if (currentUserId != id && role != SD.Role_Admin)
                    {
                        return Unauthorized();
                    }
                }

                var customer = await _customerService.UpdateCustomer(id, updateCustomerDTO, currentPassword, newPassword);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating customer with ID: {id}");
                return StatusCode(500, "Internal Server Error");
            }

        }


        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpDelete("{userId}")]
        public async Task<ActionResult<CustomerDTO>> DeleteCustomer(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest();
                }
                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    // Check if the current user is the same as the specified id or is an admin
                    if (currentUserId != userId && role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }

                var deletedCustomer = await _customerService.DeleteCustomer(userId);
                if (deletedCustomer == null)
                {
                    _logger.LogWarning("Customer not found with ID: {UserId}", userId);
                    return NotFound();
                }

                return Ok(deletedCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID: {userId}");
                return StatusCode(500, "Internal Server Error");
            }

        }


    }
}
