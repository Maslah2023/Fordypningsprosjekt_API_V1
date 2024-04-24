using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface ICartItemRepo
    {
        Task<CartItem> GetItemInCartAsync(int menuItemId);
        void AddItemToCar(CartItem cartItem);
        void RemoveItemInCart(CartItem cartItem);
    }
}
