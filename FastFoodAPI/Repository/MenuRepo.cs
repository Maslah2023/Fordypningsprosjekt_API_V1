using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Repository
{
    public class MenuRepo : IMenuRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MenuRepo> _logger;

        public MenuRepo(ApplicationDbContext db, ILogger<MenuRepo> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<MenuItem> AddMenu(MenuItem item)
        {
            try
            {
                _db.Menu.Add(item);
                await _db.SaveChangesAsync();
                return item;
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
                var menuItem = await _db.Menu.FindAsync(menuId);
                if (menuItem == null)
                {
                    return null;
                }

                _db.Menu.Remove(menuItem);
                await _db.SaveChangesAsync();
                return menuItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a menu item.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenues()
        {
            try
            {
                var menuItems = await _db.Menu.ToListAsync();
                return menuItems;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all menu items.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public async Task<MenuItem> GetMenuById(int menuId)
        {
            try
            {
                var menu = await _db.Menu.AsNoTracking().FirstOrDefaultAsync(u => u.Id == menuId);
                return menu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the menu item with ID: {menuId}.");
                return null; // or throw ex; depending on your requirements
            }
        }

        public void UpdateMenu(int menuId, MenuItem menuItem)
        {
            try
            {
                var menuToUpdate = _db.Menu.Find(menuId);

                if (menuToUpdate != null)
                {
                    menuToUpdate.Id = menuId;
                    menuToUpdate.Name = menuItem.Name;
                    menuToUpdate.Description = menuItem.Description;
                    menuToUpdate.Category = menuItem.Category;
                    menuToUpdate.Price = menuItem.Price;
                    menuToUpdate.SpecialTag = menuItem.SpecialTag;

                    _db.Menu.Update(menuToUpdate);
                    _db.SaveChanges();
                }
                else
                {
                    _logger.LogWarning($"Menu with ID {menuId} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the menu item with ID: {menuId}.");
                // You may want to throw the exception here depending on your requirements
            }
        }
    }
}
