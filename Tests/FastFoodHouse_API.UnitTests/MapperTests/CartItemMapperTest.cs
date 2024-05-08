using AutoMapper;
using FastFoodHouse_API.Mapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Xunit;

namespace FastFoodHouse_API.UniTests.MapperTest
{
    public class CartItemMapperTest
    {
        private readonly IMapper _mapper;

        public CartItemMapperTest()
        {
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new CartItemMapper()));
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void MapToDto_WhenCartItem_Given_ShouldReturn_CartItemDTO()
        {
            // Arrange
            CartItem cartItem = new CartItem
            {
                id = 1,
                MenuItemId = 1,
                Quantity = 2,
                ShoppingCartId = 1,
                MenuItems = new MenuItem
                {
                    Id = 1,
                    Name = "Test Item",
                    Category = "Test Category",
                    Description = "Test Description",
                    SpecialTag = "Test Special Tag",
                    Image = "test.jpg",
                    Price = 10
                }
            };

            // Act
            var cartItemDto = _mapper.Map<CartItemDTO>(cartItem);

            // Assert
            Assert.Equal(cartItem.id, cartItemDto.id);
            Assert.Equal(cartItem.MenuItemId, cartItemDto.MenuItemId);
            Assert.Equal(cartItem.Quantity, cartItemDto.Quantity);
            Assert.Equal(cartItem.ShoppingCartId, cartItemDto.ShoppingCartId);
            Assert.Equal(cartItem.MenuItems.Id, cartItemDto.MenuItem.Id);
            Assert.Equal(cartItem.MenuItems.Name, cartItemDto.MenuItem.Name);
            Assert.Equal(cartItem.MenuItems.Category, cartItemDto.MenuItem.Category);
            Assert.Equal(cartItem.MenuItems.Description, cartItemDto.MenuItem.Description);
            Assert.Equal(cartItem.MenuItems.SpecialTag, cartItemDto.MenuItem.SpecialTag);
            Assert.Equal(cartItem.MenuItems.Image, cartItemDto.MenuItem.Image);
            Assert.Equal(cartItem.MenuItems.Price, cartItemDto.MenuItem.Price);
        }
    }
}
