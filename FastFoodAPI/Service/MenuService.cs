using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Service
{
    public class MenuItemService : IMenuService
    {
        private readonly IMenuRepo _menuRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuItemService> _logger;

        public MenuItemService(IMenuRepo menuRepo, IMapper mapper, ILogger<MenuItemService> logger)
        {
            _menuRepo = menuRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MenuItem> AddMenu(CreateMenuDTO item)
        {
            try
            {
                var menuItem = _mapper.Map<MenuItem>(item);
                MenuItem menuItemDTO = _mapper.Map<MenuItem>(await _menuRepo.AddMenu(menuItem));
                return menuItemDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a menu item.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public async Task<MenuItem> DeleteMenu(int menuId)
        {
            try
            {
                MenuItem menuItem = _mapper.Map<MenuItem>(await _menuRepo.DeleteMenu(menuId));
                return menuItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a menu item.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuesAsync()
        {
            try
            {
                IEnumerable<MenuItem> menuDTO = _mapper.Map<IEnumerable<MenuItem>>(await _menuRepo.GetAllMenues());
                return menuDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all menu items.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public async Task<MenuItemDTO> GetMenuByIdAsync(int menuId)
        {
            try
            {
                MenuItemDTO menuDTO = _mapper.Map<MenuItemDTO>(await _menuRepo.GetMenuById(menuId));
                return menuDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the menu item with ID: {menuId}.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public void UpdateMenu(int menuId, MenuUpdateDTO menuUpdateDTO)
        {
            try
            {
                MenuItem menuItem = _mapper.Map<MenuItem>(menuUpdateDTO);
                _menuRepo.UpdateMenu(menuId, menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the menu item with ID: {menuId}.");
                // You may want to throw the exception here depending on your requirements
            }
        }
    }
}
