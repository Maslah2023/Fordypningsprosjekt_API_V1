namespace FastFoodHouse_API.Models.Dtos
{
    public class MenuUpdateDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? SpecialTag { get; set; }
        public double Price { get; set; }
        public string? Image { get; set; }
    }
}
