using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FastFoodHouse_API.Controller
{
    [Route("api/v1/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _apiResponse;
        private readonly IMenuService _menuService;
        private readonly ILogger<MenuController> _logger;

        // Constructor: Initializes the MenuController with an instance of ApplicationDbContext.
        // Also initializes an ApiResponse object for handling API responses.
        public MenuController(ApplicationDbContext db, IMenuService menuService, ILogger<MenuController> logger)
        {
            _db = db;
            _apiResponse = new ApiResponse();
            _menuService = menuService;
            _logger = logger;
        }


       [HttpGet]

       public async Task<ActionResult<ApiResponse>> GetAllMenuItems()
       {
            try
            {
                var menuItems = await _menuService.GetAllMenuesAsync();
                if(menuItems == null)
                {
                    return NotFound();
                }
                _apiResponse.Result = menuItems;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal Server Error");
                return BadRequest();
            }
            
        }



        [HttpGet("{id}")]

        public async Task<ActionResult<MenuItem>> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await
                _menuService.GetMenuByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound();
                }
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal Server Error");
                return BadRequest();
            }
            
        }


    
        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenu(CreateMenuDTO createMenuDTO)
        {
            
            try
            {
                   var menuDTO = await _menuService.AddMenu(createMenuDTO);
                    return Ok(menuDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal Server Error");
                return BadRequest();
            }
           
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenu(int id, MenuUpdateDTO menuUpdateDTO)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _menuService.UpdateMenu(id, menuUpdateDTO);
                }
                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode= HttpStatusCode.InternalServerError;
                _apiResponse.IsSuccess=false;
                _apiResponse.Message = "Internal Server Error";
            }
            return _apiResponse;
        }


        [HttpDelete("{id}")]
        public  async Task<ActionResult<MenuItem>> DeleteMenu(int id)
        {
            try
            {
               MenuItem menuDTO= await _menuService.DeleteMenu(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Internal Server Error");
                return BadRequest();
            }
        }
        
    }
}

