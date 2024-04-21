using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface  IAuthService
    {
        Task<string> Register(RegisterRequestDTO model);
        Task<LoginResponseDTO> Login(LoginRequestDTO model);
        Task<string> DeleteUser(string userId);
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetCustomerById(string id);
    }
}
