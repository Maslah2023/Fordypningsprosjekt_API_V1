﻿using System.ComponentModel.DataAnnotations;

namespace FastFoodHouse_API.Models.Dtos
{
    public class OrderDetailsDTO
    {

        [Required]
        public int OrderHeaderId { get; set; }
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = new MenuItem();
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public double Price { get; set; }
    }
}
