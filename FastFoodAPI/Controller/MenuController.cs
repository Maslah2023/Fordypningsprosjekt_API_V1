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
using System.Security.Claims;

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

       public async Task<ActionResult<MenuItemDTO>> GetAllMenuItems()
       {
            try
            {
                var menuItems = await _menuService.GetAllMenuesAsync(); 
                if (menuItems == null)
                {
                    _logger.LogWarning("No menu items found.");
                    return NotFound();
                }

                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving menu items.");
                return StatusCode(500, "Internal Server Error");
            }

        }



        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await _menuService.GetMenuByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound();
                }
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An internal server error occurred.");
                return StatusCode(500, "Internal Server Error");
            }
        }



        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenu(CreateMenuDTO createMenuDTO)
        {
            try
            {
                var menuDTO = await _menuService.AddMenu(createMenuDTO);
                return Ok("Order Created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating the menu.");
                return StatusCode(500, "Internal Server Error");
            }
        }



        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpPut("{id}")]
        public ActionResult<MenuItemDTO> UpdateMenu(int id, MenuUpdateDTO menuUpdateDTO)
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
                    if (role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }
                if (ModelState.IsValid)
                {
                    _menuService.UpdateMenu(id, menuUpdateDTO);
                    return Ok("Menu Updated successfully.");
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the menu with ID: {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }



        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MenuItem>> DeleteMenu(int id)
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
                    if (role != SD.Role_Admin)
                    {
                        return Unauthorized("Unauthorized");
                    }
                }
                var menuDTO = await _menuService.DeleteMenu(id);
                if (menuDTO == null)
                {
                    return NotFound();
                }
                return Ok("Menu Deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the menu with ID: {id}.");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}

