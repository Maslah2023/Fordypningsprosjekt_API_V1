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
        public async Task<MenuDTO> AddMenu(CreateMenuDTO item)
        {
            var menuItem = _mapper.Map<MenuItem>(item);
            MenuDTO menuItemDTO = _mapper.Map<MenuDTO>( await _menuRepo.AddMenu(menuItem));
            return menuItemDTO;
        }

        public async Task<MenuDTO> DeleteMenu(int menuId)
        {
           MenuDTO  menuItem =_mapper.Map<MenuDTO>(await _menuRepo.DeleteMenu(menuId));
            return menuItem;
        }

        public async Task<IEnumerable<MenuDTO>> GetAllMenuesAsync()
        {
            IEnumerable<MenuDTO> menuDTO = _mapper.Map<IEnumerable<MenuDTO>>(await _menuRepo.GetAllMenues()); 
            return menuDTO;
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
