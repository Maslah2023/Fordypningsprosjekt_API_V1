namespace FastFoodHouse_API.Models.Dtos
{
    public class ShoppingCartCreateDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        ICollection<CartItemDTO> CartItemDTO { get; set; }
    }
}
