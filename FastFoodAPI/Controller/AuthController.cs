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







        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetUserById(string id)
        //{
        //    var userToReturn = await _authService.GetCustomerById(id);
        //    if (userToReturn == null)
        //    {
        //        return NotFound();
        //    }
        //    _response.Result = userToReturn;
        //    return Ok(_response);
        //}


        //[HttpGet]
        //public async Task<IActionResult> GetAllUsers()
        //{
        //    var users = await _authService.GetAllUsers();
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }
        //    _response.Result = users;
        //    return Ok(_response);
        //}



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            var erroMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(erroMessage))
            {
                _response.ErrorMessages = new List<string> { erroMessage.ToString() };
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.ErrorMessages = new List<string> { erroMessage.ToString() };
                _response.IsSuccess = true;    
            }
            return BadRequest(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
             var user = await _authService.Login(model);
            if (user.User == null) 
            {
                _response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { "Username or Password is incorrect" };
            }
            else
            {
                _response.Result = user;
            }
    
            return Ok(_response);
        }



        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> DeleteUser(string userId)
        //{
        //    var user = await _authService.DeleteUser(userId);
        //    if (user == "")
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}


       



    }
}
