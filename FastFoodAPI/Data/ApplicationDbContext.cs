﻿using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace FastFoodHouse_API.Data
{
    public class ApplicationDbContext : IdentityDbContext<Customer>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }


        public DbSet<MenuItemDTO> Menu { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);
            modelbuilder.Entity<MenuItemDTO>().HasData(new MenuItemDTO
            {
                Id = 1,
                Name = "Spring Roll",   
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/spring roll.jpg",
                Price = 7.99,
                Category = "Appetizer",
                SpecialTag = ""
            }, new MenuItemDTO
            {
                Id = 2,
                Name = "Idli",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/idli.jpg",
                Price = 8.99,
                Category = "Appetizer",
                SpecialTag = ""
            }, new MenuItemDTO
            {
                Id = 3,
                Name = "Panu Puri",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/pani puri.jpg",
                Price = 8.99,
                Category = "Appetizer",
                SpecialTag = "Best Seller"
            }, new MenuItemDTO
            {
                Id = 4,
                Name = "Hakka Noodles",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/hakka noodles.jpg",
                Price = 10.99,
                Category = "Entrée",
                SpecialTag = ""
            }, new MenuItemDTO
            {
                Id = 5,
                Name = "Malai Kofta",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/malai kofta.jpg",
                Price = 12.99,
                Category = "Entrée",
                SpecialTag = "Top Rated"
            }, new MenuItemDTO
            {
                Id = 6,
                Name = "Paneer Pizza",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/paneer pizza.jpg",
                Price = 11.99,
                Category = "Entrée",
                SpecialTag = ""
            }, new MenuItemDTO
            {
                Id = 7,
                Name = "Paneer Tikka",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/paneer tikka.jpg",
                Price = 13.99,
                Category = "Entrée",
                SpecialTag = "Chef's Special"
            }, new MenuItemDTO
            {
                Id = 8,
                Name = "Carrot Love",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/carrot love.jpg",
                Price = 4.99,
                Category = "Dessert",
                SpecialTag = ""
            }, new MenuItemDTO
            {
                Id = 9,
                Name = "Rasmalai",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/rasmalai.jpg",
                Price = 4.99,
                Category = "Dessert",
                SpecialTag = "Chef's Special"
            }, new MenuItemDTO
            {
                Id = 10,
                Name = "Sweet Rolls",
                Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                Image = "https://redmangoimages.blob.core.windows.net/redmango/sweet rolls.jpg",
                Price = 3.99,
                Category = "Dessert",
                SpecialTag = "Top Rated"
            });


        }
    }
}

    