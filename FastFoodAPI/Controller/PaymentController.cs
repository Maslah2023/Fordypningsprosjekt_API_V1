using FastFoodAPI.Utility;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Controller
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IConfiguration _configuration;

        public PaymentController( IShoppingCartService shoppingCartService, IConfiguration configuration)
        {
            _configuration = configuration;
            _shoppingCartService = shoppingCartService;
        }




        [Authorize(SD.Role_Customer)]
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDTO>> MakePayment(string userId)
        {
            var userClaims = User.Claims;
            var roleClaim = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (roleClaim != null)
            {
                // Get the role value
                var role = roleClaim.Value;

                // Check if the current user is the same as the specified id or is an admin
                if (currentUserId != userId)
                {
                    return Unauthorized("Unauthorized");
                }
            }
            ShoppingCartDTO shoppingCart = await _shoppingCartService.GetShoppingCart(userId);

            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
            {
                return BadRequest("Invalid shopping cart or no items in the cart.");
            }

            double cartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions()
            {
                Amount = (int)(cartTotal * 100),
                Currency = "nok",
                PaymentMethodTypes = new List<string> { "card" }
            };

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);

            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;

            

            return Ok(shoppingCart);
        }
    }
}
