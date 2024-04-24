﻿using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFoodHouse_API.Models
{
    public class CartItem
    {
        public int id {  get; set; } 
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemId")]
        public MenuItem MenuItem { get; set; } = new MenuItem();
        public int Quantity  { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
