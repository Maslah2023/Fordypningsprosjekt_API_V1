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
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controllers
{
    [Route("api/shoppingcart")]
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

        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID cannot be null or empty.");
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

        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> AddOrUpdateCart(string userId, int menuItemId, int updateQuantityBy)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || updateQuantityBy == 0)
                {
                    return BadRequest("User ID and update quantity must be provided and greater than 0.");
                }

                ShoppingCartDTO shoppingCart = await _shoppingCartService.GetShoppingCart(userId);
                if (shoppingCart == null && updateQuantityBy > 0)
                {
                    ShoppingCartCreateDTO newCart = new ShoppingCartCreateDTO() { UserId = userId };
                    shoppingCart = await _shoppingCartService.CreateShoppingCart(newCart);
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
                    }
                    else
                    {
                        int newQuantity = itemInCart.Quantity + updateQuantityBy;

                        if (newQuantity <= 0)
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

                    return Ok(shoppingCart);
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
