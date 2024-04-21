using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FastFoodHouse_API.Controller
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        protected ApiResponse _apiResponse;

        public PaymentController(ApplicationDbContext db, IConfiguration configuration)
        {
            _configuration = configuration;
            _db = db;
            _apiResponse = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId)
        {
            ShoppingCart? shoppingCart = await
           _db.ShoppingCarts.Include(u => u.CartItems)
           .ThenInclude(u => u.MenuItem).FirstOrDefaultAsync(u => u.UserId == userId);

            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0)
            {
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                return BadRequest(_apiResponse);
            }

            double cartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.MenuItem.Price);
   

            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions()
            {
                Amount = (int)(cartTotal*100),
                Currency = "nok",
                PaymentMethodTypes = new List<string>
                {
                    "Card"
                }
                
            };

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);
            shoppingCart.StripePaymentIntentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;
            _apiResponse.Result = shoppingCart;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return _apiResponse;
          }
    }
}
