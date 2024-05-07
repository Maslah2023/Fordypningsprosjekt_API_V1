using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface

{
    public interface IShoppingCartRepo
    {
        Task<ShoppingCart> GetShoppingCart(string userId);
        Task<ShoppingCart> GetShoppingCartById(string userId);
        Task<ShoppingCart> CreateShoppingCart(ShoppingCart shoppingCart);
        void RemoveCart(ShoppingCart shoppingCart);

        void UpdateShoppingCart(ShoppingCart shoppingCart);
    }
}
