using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models.Dtos
{
    public class CartItemCreateDTO
    {
        public int id { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }

        public static implicit operator CartItemCreateDTO(CartItem v)
        {
            throw new NotImplementedException();
        }
    }
}
