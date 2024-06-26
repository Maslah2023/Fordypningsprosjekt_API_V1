﻿using Microsoft.AspNetCore.Identity;

namespace FastFoodHouse_API.Models
{
    public class Customer : IdentityUser
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}
