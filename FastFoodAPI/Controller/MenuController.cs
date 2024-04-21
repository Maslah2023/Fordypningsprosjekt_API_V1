using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
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

        // Constructor: Initializes the MenuController with an instance of ApplicationDbContext.
        // Also initializes an ApiResponse object for handling API responses.
        public MenuController(ApplicationDbContext db)
        {
            _db = db;
            _apiResponse = new ApiResponse();
        }

        //// HTTP GET endpoint for retrieving menu items.
        //[HttpGet]
        //public IActionResult GetMenuItems()
        //{
        //    try
        //    {
        //        // Try to retrieve menu items from the database and assign them to ApiResponse.Result.
        //        _apiResponse.Result = _db.Menu;

        //        // Set the HTTP status code to 200 OK.
        //        _apiResponse.StatusCode = HttpStatusCode.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        // If an exception occurs, set ApiResponse properties to indicate an error.
        //        _apiResponse.IsSuccess = false;
        //        _apiResponse.ErrorMessages = new List<string> { ex.Message };
        //    }

        //    // Return an OkObjectResult with the ApiResponse, which includes menu items or error information.
        //    return Ok(_apiResponse);
        //}


        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMenuItemById(int id)
        //{
        //    if(id == 0) 
        //    {
        //        _apiResponse.IsSuccess=false;
        //        _apiResponse.StatusCode=HttpStatusCode.NotFound;
        //        return Ok(_apiResponse);
        //    }

        //        _apiResponse.StatusCode =HttpStatusCode.OK;
        //        _apiResponse.Result = await _db.Menu.FindAsync(id);

        //    return Ok(_apiResponse);
        //}


       [HttpGet]

       public async Task<IActionResult> GetAllMenuItems()
       {
            try
            {
                _apiResponse.Result = _db.Menu;
                _apiResponse.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages =  new List<string> { ex.Message };
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_apiResponse);
            }

            return Ok(_apiResponse);
       }



        [HttpGet("{id}")]

        public async Task<IActionResult> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await 
                _db.Menu.FirstOrDefaultAsync(x => x.Id == id);
                if (menuItem == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode= HttpStatusCode.NotFound;
                    return BadRequest(_apiResponse);
                }
                _apiResponse.Result = menuItem;
                _apiResponse.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess=false;
                _apiResponse.ErrorMessages = new List<string> { ex.Message};
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                return BadRequest(_apiResponse);
            }
            return Ok(_apiResponse);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMenu(CreateMenuDTO createMenuDTO)
        {
            MenuItem newMenuItem;
            try 
            { 
            if(createMenuDTO == null) 
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return _apiResponse;
            }

             newMenuItem = new MenuItem()
            {
                Name = createMenuDTO.Name,
                Description = createMenuDTO.Description,
                Image = createMenuDTO.Image,
                Price = createMenuDTO.Price,
                Category = createMenuDTO.Category,
                SpecialTag = createMenuDTO.SpecialTag
            };
            _db.Menu.Add(newMenuItem);
             await _db.SaveChangesAsync();
            } catch (Exception ex)
            {
                _apiResponse.IsSuccess=false;
                _apiResponse.StatusCode=HttpStatusCode.InternalServerError;
                _apiResponse.ErrorMessages = new List<string> { ex.Message};
                return _apiResponse;
            }
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return _apiResponse;
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateMenu(int id, MenuUpdateDTO menuUpdateDTO)
        {
            try
            {
                if (menuUpdateDTO == null || id != menuUpdateDTO.Id)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.IsSuccess = false;
                    return _apiResponse;
                }

                MenuItem menuItem = _db.Menu.Find(id);
                if (menuItem == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.IsSuccess = false;
                    return _apiResponse;
                }
                menuItem.Id= menuUpdateDTO.Id;
                menuItem.Name = menuUpdateDTO.Name;
                menuItem.Price = menuUpdateDTO.Price;
                menuItem.Description = menuUpdateDTO.Description;
                menuItem.Category = menuUpdateDTO.Category;
                menuItem.Image = menuUpdateDTO.Image;
                menuItem.SpecialTag = menuUpdateDTO.SpecialTag;


                _db.Menu.Update(menuItem);
                await _db.SaveChangesAsync();

                _apiResponse.StatusCode = HttpStatusCode.NoContent;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode= HttpStatusCode.InternalServerError;
                _apiResponse.IsSuccess=false;
                _apiResponse.ErrorMessages.Add(ex.Message);

            }
            return _apiResponse;
           


        }
        
    }
}

