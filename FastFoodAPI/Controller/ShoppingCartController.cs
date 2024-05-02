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

        private readonly IShoppingCartService _shoppingCartService;
        private readonly ApiResponse _response;
        private readonly IMenuService _menuService;
        private readonly ICartItemService _cartItemService;
        public ShoppingCartController(IShoppingCartService shoppingCartService, IMenuService menuService, ICartItemService cartItemService)
        {
            _shoppingCartService = shoppingCartService;
            _menuService = menuService;
            _response = new ApiResponse();
            _cartItemService = cartItemService;
        }
  


        [Authorize]
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

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> AddOrUpdateCart(string userId, int menuItemId, int updateQuatityBy)
        {
            try
            {
                //ShoppingCart? shoppingCart = await
                //_db.ShoppingCarts
                //.Include(u => u.CartItems)
                //.FirstOrDefaultAsync(u => u.UserId == userId);
               ShoppingCartDTO shoppingCartDTO = await _shoppingCartService.GetShoppingCart(userId);
          
          

                MenuDTO? menuItem = await
               _menuService.GetMenuByIdAsync(menuItemId);

                if (menuItem == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (shoppingCartDTO == null && updateQuatityBy > 0)
                {
                    ShoppingCartCreateDTO newCart = new ShoppingCartCreateDTO() { UserId = userId };
                    _shoppingCartService.CreateShoppingCart(newCart);

                    CartItemCreateDTO newItem = new CartItemCreateDTO()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuatityBy,
                        ShoppingCartId = newCart.Id,
                    };
                    _cartItemService.AddItemToCart(newItem);
                    
                }
                else
                {
                    // Shopping cart exist
                    CartItemDTO? itemInCart = shoppingCartDTO.CartItemDTO.SingleOrDefault(u => u.MenuItemId == menuItemId)!;
                    // Item does not exits in current cart
                    if (itemInCart == null)
                    {
                        CartItemCreateDTO newItem = new CartItemCreateDTO()
                        {
                            MenuItemId = menuItemId,
                            Quantity = updateQuatityBy,
                            ShoppingCartId = shoppingCartDTO.Id,
                        };
                        _cartItemService.AddItemToCart(newItem);
                    }
                    else
                    {
                        // item exist in current cart
                        int newQuantity = itemInCart.Quantity + updateQuatityBy;
                        if (updateQuatityBy == 0 || newQuantity <= 0)
                        {
                            // remove item from the cart and if it is the only item then remove the shoppingcart
                            _cartItemService.RemoveItemInCart(itemInCart);
                            //_db.CartItems.Remove(itemInCart);
                            if (shoppingCartDTO.CartItemDTO.Count() == 0)
                            {
                                //_db.ShoppingCarts.Remove(shoppingCart);
                                _shoppingCartService.RemoveCart(shoppingCartDTO);
                            }
                        }
                        else
                        {
                            itemInCart.Quantity = newQuantity;
                        }
                        //await _db.SaveChangesAsync();
                        _shoppingCartService.SaveChangesAsync();
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


        //[HttpPost]

        //public async Task<IActionResult> AddShoppingCart(string userId, int menuItemId,  int updateQuantityBy)
        //{
        //    ShoppingCartCreateDTO createShoopingCartDTO;
        //    IEnumerable<ShoppingCartDTO> shoppingCartDTO = await _shoppingCartService.GetShoppingCart(userId);

        //    MenuDTO  menuItem = await _menuService.GetMenuByIdAsync(menuItemId);

        //    if (menuItem == null)
        //    {
        //        _response.StatusCode=HttpStatusCode.NotFound;
        //        _response.IsSuccess = false;
        //        return NotFound(_response);
        //    }

        //    if (shoppingCartDTO == null && updateQuantityBy > 0)
        //    {
        //        ShoppingCartCreateDTO addOrUpdateShoppingCart = new() { UserId = userId };

        //        createShoopingCartDTO = await _shoppingCartService.AddOrUpdateShoppingCart(addOrUpdateShoppingCart);

        //        CartItemCreateDTO cartDTO = new CartItemCreateDTO()
        //        {
        //            MenuItemId = menuItemId,
        //            Quantity = updateQuantityBy,
        //            ShoppingCartId = createShoopingCartDTO.Id
        //        };


        //        _cartItemService.AddItemToCart(cartDTO);

        //    }
        //    else
        //    {

        //        CartItemDTO cartItemDTO = await _cartItemService.GetCartItemById(menuItemId);
        //        IEnumerable<ShoppingCartDTO> getshoppingCartDTO = await _shoppingCartService.GetShoppingCart(createShoopingCartDTO.UserId);
        //        if(cartItemDTO == null)
        //        {
        //            cartItemDTO = new CartItemDTO()
        //            {
        //                MenuItemId = menuItemId,
        //                Quantity = updateQuantityBy,
        //                ShoppingCartId = createShoopingCartDTO.Id,

        //            };
        //        }
        //    }
        //    return Created();

        //}



    }
}
