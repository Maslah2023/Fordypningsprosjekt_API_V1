using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface

{
    public interface IShoppingCartRepo
    {
        Task<ShoppingCart> GetShoopingCart(string userId);
        Task<ShoppingCart> GetShoppingCartById(string userId);
        void CreateShoppingCart(ShoppingCart shoppingCart);
        void RemoveCart(ShoppingCart shoppingCart);
        void SaveChangesAsync();
    }
}
