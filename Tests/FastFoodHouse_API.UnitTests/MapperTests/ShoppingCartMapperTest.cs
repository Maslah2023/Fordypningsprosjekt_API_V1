using AutoMapper;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Mapper;

public class ShoppingCartMapperTest
{
    private readonly IMapper _mapper;

    public ShoppingCartMapperTest()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ShoppingCartMapper());
            cfg.AddProfile(new CartItemMapper()); // Add profile for CartItem mapping
        });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void MapToDto_GetShoppingCart_ShouldReturn_ShoppingCartDTO()
    {
        // Arrange
        ShoppingCart shopping = new ShoppingCart
        {
            Id = 1,
            UserId = "test",
            CartItems = new List<CartItem>
            {
                new CartItem
                {
                    id = 1,
                    MenuItemId = 1,
                    Quantity = 1,
                    ShoppingCartId = 1,
                    MenuItems = new MenuItem()
                    {
                        Id = 1,
                        Name = "test",
                        Category = "test",
                        Description = "test",
                        SpecialTag = "test",
                        Image = "test.jpg",
                        Price = 10,
                    },
                }
            }
        };

        // Act
        var shoppingCartDto = _mapper.Map<ShoppingCartDTO>(shopping);

        // Assert
        Assert.Equal(shopping.Id, shoppingCartDto.Id);
        Assert.Equal(shopping.UserId, shoppingCartDto.UserId);
        Assert.Equal(shopping.CartItems.Count(), shoppingCartDto.CartItems.Count());
        Assert.Equal(shopping.CartItems.First().id, shoppingCartDto.CartItems.First().id);
        Assert.Equal(shopping.CartItems.First().MenuItemId, shoppingCartDto.CartItems.First().MenuItemId);
        Assert.Equal(shopping.CartItems.First().Quantity, shoppingCartDto.CartItems.First().Quantity);
        // Assert other properties as needed
    }
}
