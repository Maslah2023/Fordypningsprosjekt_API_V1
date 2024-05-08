using FastFoodAPI.Utility;
using FastFoodHouse_API.Controller;
using FastFoodHouse_API.Controllers;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace FastFoodHouse_API.UnitTests.ControllerTests
{
    public class PaymentControllerTests
    {
        private readonly PaymentController _controller;
        private readonly Mock<IShoppingCartService> _shoppingCartMock = new Mock<IShoppingCartService>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();

        public PaymentControllerTests()
        {
            _controller = new PaymentController(_shoppingCartMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task MakePayment_ReturnsOk_WhenPaymentSuccessful()
        {
            // Arrange
            var userId = "123";
            var userClaims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var userIdentity = new ClaimsIdentity(userClaims);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var httpContext = new DefaultHttpContext();
            httpContext.User = userPrincipal;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            _controller.ControllerContext = controllerContext;

            // Create a ShoppingCart instance with necessary data
            var shoppingCartDTO = new ShoppingCartDTO
            {
                UserId = userId,
                CartItems = new List<CartItemDTO> { new CartItemDTO { Quantity = 2, MenuItem = new MenuItemDTO { Price = 10.0 } } }
            };

            _shoppingCartMock.Setup(x => x.GetShoppingCart(userId)).ReturnsAsync(shoppingCartDTO);

            // Mock the configuration settings for Stripe
            _configurationMock.Setup(x => x["StripeSettings:SecretKey"]).Returns("sk_test_51P2wozICpKBIbkN8g4Ajn0X28pk5zRuhl5f86UBsYu1elNLIhxhon5dAvCs56MuZ6mIAj749cdTUMW4a3o8ygsVq00Z82MJq0Z");

            // Mock the Stripe API call
            var paymentIntentServiceMock = new Mock<Stripe.PaymentIntentService>();
            var paymentIntentMock = new Stripe.PaymentIntent { Id = "test_payment_intent_id", ClientSecret = "test_client_secret" };
            paymentIntentServiceMock.Setup(x => x.Create(It.IsAny<Stripe.PaymentIntentCreateOptions>(), null)).Returns(paymentIntentMock);
            Stripe.StripeConfiguration.SetApiKey("your_stripe_secret_key");

            // Act
            var result = await _controller.MakePayment(userId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var shoppingCartResult = Assert.IsType<ShoppingCart>(actionResult.Value);
            Assert.Equal(userId, shoppingCartResult.UserId);
        }

        // Additional test methods can be added here
    }
}
