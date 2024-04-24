using FastFoodHouse_API.Models;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface IMenuRepo
    {
        Task<MenuItem> GetAllMenues();
        Task<MenuItem> GetMenuById(int menuName);
        void AddMenu(MenuItem item);
        void UpdateMenu(int menuId);
        void DeleteMenu(int menuId);
        

        
    }
}
