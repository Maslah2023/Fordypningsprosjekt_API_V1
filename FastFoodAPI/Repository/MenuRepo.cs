using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;

namespace FastFoodHouse_API.Repository
{
    public class MenuRepo : IMenuRepo
    {
        private readonly ApplicationDbContext _db;

        public MenuRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddMenu(MenuItemDTO item)
        {
            MenuItemDTO menuItem = new()
            {
                Id = item.Id,

            }; 
        }

        public void DeleteMenu(int menuId)
        {
            throw new NotImplementedException();
        }

        public Task<MenuItemDTO> GetAllMenues()
        {
            throw new NotImplementedException();
        }

        public async Task<MenuItemDTO> GetMenuById(int menuId)
        {
            MenuItemDTO? menu = await _db.Menu.FindAsync(menuId);
            return menu;
        }

        public void UpdateMenu(int menuId)
        {
            throw new NotImplementedException();
        }
    }
}
