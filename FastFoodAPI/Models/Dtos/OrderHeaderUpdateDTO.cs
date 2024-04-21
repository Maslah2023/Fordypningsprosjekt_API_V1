namespace FastFoodHouse_API.Models.Dtos
{
    public class OrderHeaderUpdateDTO
    {
        public int OrderHeaderId { get; set; }
        public string PickupName { get; set; }
        public string PickupPhoneNumber { get; set; }
        public string PickupEmail { get; set; }
        public string ApplicationUserId { get; set; }
        public int StripePaymentIntentId { get; set; }
        public string status { get; set; }
    }
}
