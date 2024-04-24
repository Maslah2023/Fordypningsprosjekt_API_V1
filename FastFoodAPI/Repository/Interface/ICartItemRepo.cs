using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface ICartItemRepo
    {
        Task<CartItem> CartItemAsync(int id);
        void AddItemToCar(CartItem cartItem);
        void RemoveItemInCart(CartItem cartItem);
    }
}
