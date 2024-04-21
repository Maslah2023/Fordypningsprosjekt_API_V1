using FastFoodHouse_API.Models;


namespace FastFoodHouse_API.Service.Interface
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Customer applicationUser, IEnumerable<string> roles);
    }
}
