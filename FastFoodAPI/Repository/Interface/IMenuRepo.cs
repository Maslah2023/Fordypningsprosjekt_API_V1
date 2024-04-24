using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface IMenuRepo
    {
        Task<MenuItemDTO> GetAllMenues();
        Task<MenuItemDTO> GetMenuById(int menuName);
        void AddMenu(MenuItemDTO item);
        void UpdateMenu(int menuId);
        void DeleteMenu(int menuId);
        

        
    }
}
