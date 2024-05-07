using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;
using System;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepo _shoppingCartRepo;
        private readonly IMapper _mapper;

        public ShoppingCartService(IShoppingCartRepo shoppingCartRepo, IMapper mapper)
        {
            _shoppingCartRepo = shoppingCartRepo ?? throw new ArgumentNullException(nameof(shoppingCartRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ShoppingCartDTO> CreateShoppingCart(ShoppingCartCreateDTO shoppingCartCreateDTO)
        {
            try
            {
                ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartCreateDTO);
                ShoppingCartDTO shoppingCartDto = _mapper.Map<ShoppingCartDTO>(await _shoppingCartRepo.CreateShoppingCart(shoppingCart));
                return shoppingCartDto;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while creating shopping cart.", ex);
            }
        }

        public async Task<ShoppingCartDTO> GetShoppingById(string id)
        {
            try
            {
                ShoppingCart shoppingCart = await _shoppingCartRepo.GetShoppingCartById(id);
                ShoppingCartDTO shoppingCartDTO = _mapper.Map<ShoppingCartDTO>(shoppingCart);
                return shoppingCartDTO;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while getting shopping cart by ID.", ex);
            }
        }

        public async Task<ShoppingCartDTO> GetShoppingCart(string userId)
        {
            try
            {
                var shoppingCarts = await _shoppingCartRepo.GetShoppingCart(userId);
                ShoppingCartDTO shoppingCartDTO = _mapper.Map<ShoppingCartDTO>(shoppingCarts);
                return shoppingCartDTO;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while getting shopping cart.", ex);
            }
        }

        public void RemoveCart(ShoppingCartDTO shoppingCartDto)
        {
            try
            {
                ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDto);
                _shoppingCartRepo.RemoveCart(shoppingCart);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while removing shopping cart.", ex);
            }
        }

     

        public void UpdateShoppingCart(ShoppingCartDTO shoppingCartDTO)
        {
            try
            {
                ShoppingCart shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDTO);
                _shoppingCartRepo.UpdateShoppingCart(shoppingCart);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while updating shopping cart.", ex);
            }
        }
    }
}
