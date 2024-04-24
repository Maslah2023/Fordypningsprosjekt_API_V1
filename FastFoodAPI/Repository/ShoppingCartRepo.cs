using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public void CreateShoppingCart(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Add(shoppingCart);
            _db.SaveChanges();
        }

        public async Task<ShoppingCart> GetShoopingCart(string userId)
        {
            ShoppingCart? shoppingCarts = await
           _db.ShoppingCarts.Include(u => u.CartItems).ThenInclude(u => u.MenuItem).FirstOrDefaultAsync(u => u.UserId == userId);
            return shoppingCarts;
         }

        public async Task<ShoppingCart> GetShoppingCartById(string userId)
        {
            return await _db.ShoppingCarts.Include(u => u.CartItems).ThenInclude(u => u.MenuItem).FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public void RemoveCart(ShoppingCart shoppingCart)
        {
            _db.ShoppingCarts.Remove(shoppingCart);
            _db.SaveChanges();
        }

        public void SaveChangesAsync()
        {
            _db?.SaveChangesAsync();
        }
    }
}
