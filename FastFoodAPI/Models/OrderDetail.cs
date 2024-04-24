using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FastFoodHouse_API.Models
{
    public class OrderDetail
    {


        [Key]
        public int OrderDetailsId { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }
        [Required]
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItemDTO? MenuItem { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string? ItemName { get; set; }
        [Required]
        public double Price { get; set; }
        //[Key]
        //public int OrderDetailId { get; set; }
        //[Required]
        //public int OrderHeaderId { get; set; }
        //[Required]
        //public int MenuItemId { get; set; }
        //[ForeignKey("MenuItemId")]
        //public MenuItem MenuItem { get; set; }
        //[Required]
        //public int Quantity { get; set; }
        //[Required]
        //public string ItemName { get; set; }
        //[Required]
        //public double Price { get; set; }
    }
}
