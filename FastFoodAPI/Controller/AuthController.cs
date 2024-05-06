using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace FastFoodHouse_API.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApiResponse _response;
        private readonly ICustomerService _customerService;

        public AuthController( IAuthService authService, ICustomerService customerService)
        {
            _authService = authService;
            _response = new ApiResponse();
            _customerService = customerService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            var erroMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(erroMessage))
            {
                _response.Message = erroMessage;
                return BadRequest(_response);
            }
            else
            {
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.Message = erroMessage;
                _response.IsSuccess = true;    
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
             var user = await _authService.Login(model);
            if (user.User == null) 
            {
                _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.Message = "Username or Password is incorrect";
            }
            else
            {
                _response.Result = user;
            }
    
            return Ok(_response);
        }


    }
}
