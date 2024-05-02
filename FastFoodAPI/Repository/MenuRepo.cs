using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

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
        public async Task<MenuItem>AddMenu(MenuItem item)
        {
            try
            {
                _db.Menu.Add(item);
               await _db.SaveChangesAsync();
                return item;
            }
            catch(Exception ex) 
            {
               _logger.LogError(ex, "Internal Server Error");
                throw;
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
                _logger.LogError(ex, "Internal Server Error");
                throw;
            }
          
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenues()
        {
            var  menuItems =  await _db.Menu.ToListAsync();
            return menuItems;
        }

        public async Task<MenuItem> GetMenuById(int menuId)
        {
            MenuItem? menu = await _db.Menu.FindAsync(menuId);
            return menu;
        }

        public void UpdateMenu(int menuId)
        {
            throw new NotImplementedException();
        }
    }
}
