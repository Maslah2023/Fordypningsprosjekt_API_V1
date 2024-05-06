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
        public async Task<MenuItem> AddMenu(CreateMenuDTO item)
        {
            var menuItem = _mapper.Map<MenuItem>(item);
            MenuItem menuItemDTO = _mapper.Map<MenuItem>( await _menuRepo.AddMenu(menuItem));
            return menuItemDTO;
        }

        public async Task<MenuItem> DeleteMenu(int menuId)
        {
           MenuItem  menuItem =_mapper.Map<MenuItem>(await _menuRepo.DeleteMenu(menuId));
            return menuItem;
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuesAsync()
        {
            IEnumerable<MenuItem> menuDTO = _mapper.Map<IEnumerable<MenuItem>>(await _menuRepo.GetAllMenues()); 
            return menuDTO;
        }

        public async Task<MenuItemDTO> GetMenuByIdAsync(int menuId)
        {
            MenuItemDTO menuDTO = _mapper.Map<MenuItemDTO>(await _menuRepo.GetMenuById(menuId)); 
            return menuDTO;
        }

        public void UpdateMenu(int menuId, MenuUpdateDTO menuUpdateDTO)
        {
            MenuItem menuItem = _mapper.Map<MenuItem>(menuUpdateDTO);
            _menuRepo.UpdateMenu(menuId, menuItem);
        }
    }
}
