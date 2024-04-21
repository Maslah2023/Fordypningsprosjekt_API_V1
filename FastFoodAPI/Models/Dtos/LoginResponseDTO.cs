namespace FastFoodHouse_API.Models.Dtos
{
    public class LoginResponseDTO
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
