using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDTO> GetShoppingCart(string userId);
        Task<ShoppingCartDTO> GetShoppingById(string id);
        void CreateShoppingCart(ShoppingCartCreateDTO shoppingCartCreateDTO);
        void RemoveCart(ShoppingCartDTO shoppingCartDto);
        void SaveChangesAsync();

    }
}
