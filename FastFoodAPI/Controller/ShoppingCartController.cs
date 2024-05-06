using AutoMapper;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Net;

namespace FastFoodHouse_API.Controller
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ApiResponse _response;
        private readonly IMenuService _menuService;
        private readonly ICartItemService _cartItemService;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IMenuService menuService, ICartItemService cartItemService, ApplicationDbContext db, ApplicationDbContext dbContext, IMapper mapper)
        {
            _shoppingCartService = shoppingCartService;
            _menuService = menuService;
            _response = new ApiResponse();
            _cartItemService = cartItemService;
            _db = db;
            _dbContext = dbContext;
            _mapper = mapper;
        }
  


        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart(string userId)
        {
        

            try
            {


                ShoppingCartDTO shoppingCarts = await
                _shoppingCartService.GetShoppingById(userId);

                if (shoppingCarts == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = shoppingCarts;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(shoppingCarts);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.Message = "Internal Server Error";
                return BadRequest(_response);
            }

           



        }

        //[Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> AddOrUpdateCart(string userId, int menuItemId, int updateQuantityBy)
        {
            try
            {
               
               ShoppingCartDTO shoppingCartDTO = await _shoppingCartService.GetShoppingCart(userId);
          
        
                MenuItemDTO menuItem = await
               _menuService.GetMenuByIdAsync(menuItemId);

                if (menuItem == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (shoppingCartDTO == null && updateQuantityBy > 0)
                {
                    ShoppingCartCreateDTO newCart = new ShoppingCartCreateDTO() { UserId = userId };
                    _shoppingCartService.CreateShoppingCart(newCart);

                    CartItemCreateDTO newItem = new CartItemCreateDTO()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = newCart.Id,
                    };
                    _cartItemService.AddItemToCart(newItem);
                    
                }
                else
                {
                    var itemInCart = shoppingCartDTO.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);

                    // Item does not exits in current cart
                    if (itemInCart == null)
                    {
                        CartItemCreateDTO newItem = new CartItemCreateDTO()
                        {
                            MenuItemId = menuItemId,
                            Quantity = updateQuantityBy,
                            ShoppingCartId = shoppingCartDTO.Id,
                        };
                        _cartItemService.AddItemToCart(newItem);
                    }
                    else
                    {


                        // item exist in current cart


                        int newQuantity = itemInCart.Quantity + updateQuantityBy;

                        if (updateQuantityBy == 0 || newQuantity <= 0)
                        {
                            // remove item from the cart and if it is the only item then remove the shoppingcart
                            
                                //remove cart item from cart and if it is the only item then remove cart
                                _cartItemService.RemoveItemInCart(itemInCart);
                                if (shoppingCartDTO.CartItems.Count() == 1)
                                {
                                   _shoppingCartService.RemoveCart(shoppingCartDTO);
                                }

                        }
                        else
                        {
                            itemInCart.Quantity = newQuantity;
                           _cartItemService.UpdateItemInCart(itemInCart);
                        }
                      
                    
                    }

                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = shoppingCartDTO;
                return Ok(shoppingCartDTO);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = "Internal Server Error";
            }

            return BadRequest(_response);

        }



    }
}
