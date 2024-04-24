using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface ICartItemService
    {
        Task<CartItemDTO> GetCartItemById(int id);
        void AddItemToCart(CartItemCreateDTO cartItemCreateDTO);
        void RemoveItemInCart(CartItemDTO cartItemDto);
        void UpdateItenInCart(CartItemDTO cartItemDTO);
    }
}
