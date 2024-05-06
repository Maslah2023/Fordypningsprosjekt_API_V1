namespace FastFoodHouse_API.Models.Dtos
{
    public class LoginResponseDTO
    {
        public CustomerDTO Customer { get; set; }
        public string Token { get; set; }
    }
}
