using AutoMapper;
using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controllers
{
    [Route("api/v1/shoppingcart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMenuService _menuService;
        private readonly ICartItemService _cartItemService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IMenuService menuService, ICartItemService cartItemService, IMapper mapper)
        {
            _shoppingCartService = shoppingCartService ?? throw new ArgumentNullException(nameof(shoppingCartService));
            _menuService = menuService ?? throw new ArgumentNullException(nameof(menuService));
            _cartItemService = cartItemService ?? throw new ArgumentNullException(nameof(cartItemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

      

        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Customer)]
        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID cannot be null or empty.");
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

                ShoppingCartDTO shoppingCart = await _shoppingCartService.GetShoppingCart(userId);

                if (shoppingCart == null)
                {
                    return NotFound("Shopping cart not found.");
                }

                return Ok(shoppingCart);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }



        [Authorize(Roles =SD.Role_Customer)]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> AddOrUpdateCart(string userId, int menuItemId, int updateQuantityBy)
        {
            try
            {
                // UserID and MenuItemID are required. Additionally, provide 'updateQuantityBy
               // "to add an item. To remove an item, 'updateQuantityBy' should not be included"
                if (string.IsNullOrEmpty(userId) || menuItemId == 0)
                {
                    return BadRequest();

                }

                var userClaims = User.Claims;
                var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                if (roleClaim != null)
                {
                    // Get the role value
                    var role = roleClaim.Value;

                    // Check if the current user is the same as the specified id or is an admin
                    if (role == SD.Role_Admin)
                    {
                        return Unauthorized("Not Allowed");
                    }
                }

                ShoppingCartDTO shoppingCart = await _shoppingCartService.GetShoppingCart(userId);
                if (shoppingCart == null && updateQuantityBy > 0)
                {
                    ShoppingCartCreateDTO newCart = new ShoppingCartCreateDTO() { UserId = userId };
                    shoppingCart = await _shoppingCartService.CreateShoppingCart(newCart);
                    return Ok("Item Added To Shopping Cart");
                }

                if (shoppingCart != null)
                {
                    MenuItemDTO menuItem = await _menuService.GetMenuByIdAsync(menuItemId);

                    if (menuItem == null)
                    {
                        return NotFound("Menu item not found.");
                    }

                    CartItemDTO itemInCart = shoppingCart.CartItems.FirstOrDefault(u => u.MenuItemId == menuItemId);

                    if (itemInCart == null)
                    {
                        CartItemCreateDTO newItem = new CartItemCreateDTO()
                        {
                            MenuItemId = menuItemId,
                            Quantity = updateQuantityBy,
                            ShoppingCartId = shoppingCart.Id
                        };
                         _cartItemService.AddItemToCart(newItem);
                          return Ok("Item Added To Shopping Cart");
                    }
                    else
                    {
                        int newQuantity = itemInCart.Quantity + updateQuantityBy;

                        if (updateQuantityBy == 0 && itemInCart != null)
                        {
                             _cartItemService.RemoveItemInCart(itemInCart);
                            if (shoppingCart.CartItems.Count() == 1)
                            {
                                 _shoppingCartService.RemoveCart(shoppingCart);
                            }
                        }
                        else
                        {
                            itemInCart.Quantity = newQuantity;
                           _cartItemService.UpdateItemInCart(itemInCart);
                        }
                    }
                    return Ok("Item Deleted From Shopping Cart");
                  
                }
                else
                {
                    return NotFound("Shopping cart not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }
    }
}
