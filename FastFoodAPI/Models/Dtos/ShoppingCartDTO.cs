using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models.Dtos
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IEnumerable<CartItemDTO> CartItems { get; set;}

        [NotMapped]
        public double CartTotal { get; set; }
        [NotMapped]
        public string StripePaymentIntentId { get; set; }
        [NotMapped]
        public string ClientSecret { get; set; }
    }
}
