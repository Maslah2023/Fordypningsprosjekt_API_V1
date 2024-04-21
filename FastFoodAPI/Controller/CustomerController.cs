using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodHouse_API.Controller
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ApplicationDbContext _db;
        protected ApiResponse apiResponse;
        public CustomerController(ICustomerService customerService, ApplicationDbContext db)
        {
            _customerService = customerService;
            apiResponse = new ApiResponse();
            _db = db;
        }






        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _customerService.GetAllCustomers();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userToReturn = await _customerService.GetCustomerById(id);
            if (userToReturn == null)
            {
                return NotFound();
            }
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAsync(string id, UpdateCustomerDTO updateCustomerDTO)
        {
            _customerService.UpdateCustomer(id, updateCustomerDTO);
            return Ok();
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
                _customerService.DeleteCustomer(userId);
                await _db.SaveChangesAsync();

            }
            catch(Exception ex) 
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                apiResponse.IsSuccess = false;
                return BadRequest(apiResponse);
                
            }

            return NoContent();
            
        }


    }
}
