namespace FastFoodHouse_API.Models.Dtos
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CartItemDTO> CartItems { get; set;}
    }
}
