using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
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

        private readonly ApplicationDbContext _db;
        private readonly ApiResponse _response;
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            _response = new ApiResponse();

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
            ShoppingCart? shoppingCart;

            try
            {
                shoppingCart =
                await _db.ShoppingCarts
               .Include(u => u.CartItems)
               .FirstOrDefaultAsync(u => u.UserId == userId);

                if (string.IsNullOrEmpty(userId))
                {
                    shoppingCart = new ShoppingCart();
                }


                if (shoppingCart == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.Message };
                return BadRequest(_response);
            }

            _response.Result = shoppingCart;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);



        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddOrUpdateCart(string userId, int menuItemId, int updateQuatityBy)
        {
            try
            {
                ShoppingCart? shoppingCart = await
                _db.ShoppingCarts
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(u => u.UserId == userId);

                MenuItem? menuItem = await
               _db.Menu.FirstOrDefaultAsync(u => u.Id == menuItemId);

                if (menuItem == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (shoppingCart == null && updateQuatityBy > 0)
                {
                    ShoppingCart newCart = new ShoppingCart() { UserId = userId };
                    _db.ShoppingCarts.Add(newCart);
                    await _db.SaveChangesAsync();

                    CartItem newItem = new CartItem()
                    {
                        MenuItemId = menuItemId,
                        Quantity = updateQuatityBy,
                        ShoppingCartId = newCart.id,
                        MenuItem = null
                    };
                    _db.CartItems.Add(newItem);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Shopping cart exist
                    CartItem? itemInCart = await _db.CartItems.FirstOrDefaultAsync(u => u.MenuItemId == menuItemId);
                    // Item does not exits in current cart
                    if (itemInCart == null)
                    {
                        CartItem newItem = new CartItem()
                        {
                            MenuItemId = menuItemId,
                            Quantity = updateQuatityBy,
                            ShoppingCartId = shoppingCart.id,
                            MenuItem = null
                        };
                        _db.CartItems.Add(newItem);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        // item exist in current cart
                        int newQuantity = itemInCart.Quantity + updateQuatityBy;
                        if (updateQuatityBy == 0  || newQuantity <= 0)
                        {
                            // remove item from the cart and if it is the only item then remove the shoppingcart
                            _db.CartItems.Remove(itemInCart);
                            if(shoppingCart.CartItems.Count() == 1)
                            {
                                _db.ShoppingCarts.Remove(shoppingCart);
                            }
                            await _db.SaveChangesAsync();
                        }
                        else
                        {
                            itemInCart.Quantity = newQuantity;
                        }
                       await  _db.SaveChangesAsync();
                    }

                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = shoppingCart;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string> {ex.ToString()};
            }

            return BadRequest(_response);

        }



    }
}
