using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;

namespace FastFoodHouse_API.Service
{
    public class MenuItemService : IMenuService
    {
        private readonly IMenuRepo _menuRepo;
        private readonly IMapper _mapper;

        public MenuItemService(IMenuRepo menuRepo, IMapper mapper)
        {
            _menuRepo = menuRepo;
            _mapper = mapper;   
            
        }
        public void AddMenu(MenuItem item)
        {
            _menuRepo.AddMenu(item);
        }

        public void DeleteMenu(int menuId)
        {
            throw new NotImplementedException();
        }

        public Task<MenuDTO> GetAllMenuesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<MenuDTO> GetMenuByIdAsync(int menuId)
        {
            MenuDTO menuDTO = _mapper.Map<MenuDTO>(await _menuRepo.GetMenuById(menuId)); 
            return menuDTO;
        }

        public void UpdateMenu(int menuId)
        {
            throw new NotImplementedException();
        }
    }
}
