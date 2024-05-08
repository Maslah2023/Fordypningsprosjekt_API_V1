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
                MenuItem = new MenuItem
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
            Assert.Equal(cartItem.MenuItem.Id, cartItemDto.MenuItem.Id);
            Assert.Equal(cartItem.MenuItem.Name, cartItemDto.MenuItem.Name);
            Assert.Equal(cartItem.MenuItem.Category, cartItemDto.MenuItem.Category);
            Assert.Equal(cartItem.MenuItem.Description, cartItemDto.MenuItem.Description);
            Assert.Equal(cartItem.MenuItem.SpecialTag, cartItemDto.MenuItem.SpecialTag);
            Assert.Equal(cartItem.MenuItem.Image, cartItemDto.MenuItem.Image);
            Assert.Equal(cartItem.MenuItem.Price, cartItemDto.MenuItem.Price);
        }
    }
}
