using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models
{
    public class ShoppingCart
    {
        public int id { get; set; }
        public string UserId { get; set; }
        public ICollection<CartItem>? CartItems { get; set; }

        [NotMapped]
        public double CartTotal { get; set; }
        [NotMapped]
        public string? StripePaymentIntentId { get; set; }
        [NotMapped]
        public string? ClientSecret { get; set; }
    }
}
