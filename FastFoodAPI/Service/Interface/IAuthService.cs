using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface  IAuthService
    {
        Task<string> Register(RegisterRequestDTO model);
        Task<LoginResponseDTO> Login(LoginRequestDTO model);
    }
}
