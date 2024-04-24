using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;

namespace FastFoodHouse_API.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepo _cartItemRepo;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepo cartItemRepo, IMapper mapper)
        {
           _cartItemRepo = cartItemRepo;
            _mapper = mapper;
        }
        public void AddItemToCart(CartItemCreateDTO cartItemCreateDTO)
        {
            CartItem cartItem = _mapper.Map<CartItem>(cartItemCreateDTO);
            _cartItemRepo.AddItemToCar(cartItem);
        }

        public async Task<CartItemDTO> GetCartItemById(int id)
        {
            CartItemDTO cartItemDTO = _mapper.Map<CartItemDTO>(await _cartItemRepo.GetItemInCartAsync(id));
            return cartItemDTO;
        }

        public void RemoveItemInCart(CartItem cartItem)
        {
            _cartItemRepo.AddItemToCar(cartItem);
        }

        public void RemoveItemInCart(CartItemDTO cartItemDto)
        {
            throw new NotImplementedException();
        }

        public void UpdateItenInCart(CartItemDTO cartItemDTO)
        {
            throw new NotImplementedException();
        }
    }
}
