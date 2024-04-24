using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service;
using FastFoodHouse_API.Service.Interface;
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
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApiResponse>> GetShoppingCarts(string id)
        //{
        //    ShoppingCart? shoppingCart;
        //    if(string.IsNullOrEmpty(id)) 
        //    {
        //        shoppingCart = new();
        //    }
        //    else
        //    {
        //         shoppingCart = await _db.ShoppingCarts
        //        .Include(u => u.CartItems).ThenInclude(u => u.MenuItem)
        //        .FirstOrDefaultAsync(u => u.UserId == id);
        //    }
        //    if(shoppingCart.CartItems != null && shoppingCart.CartItems.Count > 0)
        //    {

        //    }
        //    _response.StatusCode = System.Net.HttpStatusCode.OK;
        //    _response.Result = shoppingCart;
        //    return Ok(_response);

        //}


        //[HttpPost]

        //public async Task<ActionResult<ApiResponse>> AddshoppingCartAndItem(string userId, int menuItemId, int updateQuantity)
        //{
        //    ShoppingCart? shoppingCart = _db.ShoppingCarts.Include(u => u.CartItems).FirstOrDefault(u => u.UserId == userId);
        //    MenuItem? menuItem = _db.Menu.FirstOrDefault(u => u.Id == menuItemId);
        //    if (menuItem == null) 
        //    {
        //        _response.StatusCode=System.Net.HttpStatusCode.NotFound;
        //        _response.IsSuccess = false;
        //        return Ok(_response);
        //    }
        //    if(shoppingCart == null) 
        //    {
        //        ShoppingCart newCart = new() { UserId = userId };
        //        _db.Add(newCart);
        //        await _db.SaveChangesAsync();

        //        CartItem newCartItem = new CartItem()
        //        {
        //            Quantity = updateQuantity,
        //            MenuItemId = menuItem.Id,
        //            ShoppingCartId = newCart.id

        //        };
        //        _db.Add(newCartItem);
        //        await _db.SaveChangesAsync();
        //    }
        //    return Ok(_response);
        //}



        [HttpGet]
        public async Task<IActionResult> GetShoppingCart(string userId)
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
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }

           



        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateCart(string userId, int menuItemId, int updateQuatityBy)
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
                    CartItemDTO? itemInCart = await _cartItemService.GetCartItemById(menuItemId);
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
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> { ex.ToString() };
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
