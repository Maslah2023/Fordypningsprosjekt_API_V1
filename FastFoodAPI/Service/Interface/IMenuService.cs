using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuDTO>> GetAllMenuesAsync();
        Task<MenuDTO> GetMenuByIdAsync(int menuId);
        Task<MenuDTO> AddMenu(CreateMenuDTO item);
        void UpdateMenu(int menuId);
        Task<MenuDTO> DeleteMenu(int menuId);
    }
}
