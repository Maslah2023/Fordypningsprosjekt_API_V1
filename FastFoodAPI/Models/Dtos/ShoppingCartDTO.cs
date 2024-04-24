namespace FastFoodHouse_API.Models.Dtos
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItemDTO> CartItemDTO { get; set;}
    }
}
