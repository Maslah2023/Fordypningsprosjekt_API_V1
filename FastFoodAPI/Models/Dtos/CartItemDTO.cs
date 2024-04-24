using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models.Dtos
{
    public class CartItemDTO
    {
        public int id { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuDTO MenuItemDTO { get; set; } = new MenuDTO();
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
