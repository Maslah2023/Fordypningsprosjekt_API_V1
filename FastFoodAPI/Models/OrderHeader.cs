using FastFoodHouse_API.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models
{
    public class OrderHeader
    {
        public OrderHeader()
        {
            OrderDetails = new List<OrderDetail>();
        }



        [Key]
        public int OrderheaderId {  get; set; }
        [Required]
        public string PickupName { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        [Required]
        public string PickupEmail { get; set; }
        [Required]
        


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public Customer Customer { get; set; }
        public double OrderTotal { get; set; }

        public string? StripePaymentIntentName { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string StripePaymentIntentID { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } 


        // [Key]
        //public int OrderHeaderId { get; set; }
        //[Required]
        //public string PickupName { get; set; }
        //[Required]
        //public string PickupPhoneNumber { get; set; }
        //[Required]
        //public string PickupEmail { get; set; }

        //public string ApplicationUserId { get; set; }
        //[ForeignKey("ApplicationUserId")]
        //public ApplicationUser User { get; set; }
        //public double OrderTotal { get; set; }


        //public DateTime OrderDate { get; set; }
        //public string StripePaymentIntentID { get; set; }
        //public string Status { get; set; }
        //public int TotalItems { get; set; }

        //public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
