using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuItem>> GetAllMenuesAsync();
        Task<MenuItemDTO> GetMenuByIdAsync(int menuId);
        Task<MenuItem> AddMenu(CreateMenuDTO item);
        void UpdateMenu(int menuId, MenuUpdateDTO menuUpdateDTO);
        Task<MenuItem> DeleteMenu(int menuId);
    }
}
