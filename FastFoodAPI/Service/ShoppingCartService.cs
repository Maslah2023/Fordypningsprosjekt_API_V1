using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;

namespace FastFoodHouse_API.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepo _shoppingCartRepo;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepo shoppingCartRepo, IMapper mapper)
        {
            _shoppingCartRepo = shoppingCartRepo;
            _mapper = mapper;
        }

        public void CreateShoppingCart(ShoppingCartCreateDTO shoppingCartCreateDTO)
        {
            ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartCreateDTO); 
            _shoppingCartRepo.CreateShoppingCart(shoppingCart);
        }

        public async Task<ShoppingCartDTO> GetShoppingById(string id)
        {
            ShoppingCart shoppingCart = await _shoppingCartRepo.GetShoppingCartById(id);
            ShoppingCartDTO shoppingCartDTO = _mapper.Map<ShoppingCartDTO>(shoppingCart);
            return shoppingCartDTO;
           
        }

        public async Task<ShoppingCartDTO> GetShoppingCart(string userId)
        {
          var shoppingCarts = await _shoppingCartRepo.GetShoopingCart(userId);
          ShoppingCartDTO shoppingCartDTO = _mapper.Map<ShoppingCartDTO>(shoppingCarts);
          return shoppingCartDTO;
        }

        public void RemoveCart(ShoppingCartDTO shoppingCartDto)
        {
            ShoppingCart shoppingCart = _mapper
            .Map<ShoppingCart>(shoppingCartDto);
            _shoppingCartRepo.RemoveCart(shoppingCart);
        }

        public  void SaveChangesAsync()
        {
            _shoppingCartRepo.SaveChangesAsync();
        }
    }
}
