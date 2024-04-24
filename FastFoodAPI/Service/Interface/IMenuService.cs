using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface IMenuService
    {
        Task<MenuDTO> GetAllMenuesAsync();
        Task<MenuDTO> GetMenuByIdAsync(int menuId);
        void AddMenu(MenuItemDTO item);
        void UpdateMenu(int menuId);
        void DeleteMenu(int menuId);
    }
}
