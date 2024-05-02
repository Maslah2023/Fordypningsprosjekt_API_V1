using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface IMenuRepo
    {
        Task<IEnumerable<MenuItem>> GetAllMenues();
        Task<MenuItem> GetMenuById(int menuName);
        Task<MenuItem> AddMenu(MenuItem item);
        void UpdateMenu(int menuId);
        Task<MenuItem> DeleteMenu(int menuId);
        

        
    }
}
