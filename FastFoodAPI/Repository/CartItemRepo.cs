using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository
{
    public class CartItemRepo : ICartItemRepo
    {
        private readonly ApplicationDbContext _db;

        public CartItemRepo(ApplicationDbContext db)
        {
            _db = db;
        }
        public void AddItemToCar(CartItem cartItem)
        {
           _db.CartItems.Add(cartItem);
            _db.SaveChanges();
        }



        public Task<CartItem> GetItemInCartAsync(int menuItemId)
        {
            throw new NotImplementedException();
        }

        public void RemoveItemInCart(CartItem cartItem)
        {
            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();
        }
    }
}
