using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controller
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICustomerService _customerService;

        public AuthController(IAuthService authService, ICustomerService customerService)
        {
            _authService = authService;
            _customerService = customerService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestDTO model)
        {
            try
            {
                var errorMessage = await _authService.Register(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while registering: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            try
            {
                var user = await _authService.Login(model);
                if (user.Customer == null)
                {
                    return Unauthorized("Username or Password is incorrect");
                }
                else
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
