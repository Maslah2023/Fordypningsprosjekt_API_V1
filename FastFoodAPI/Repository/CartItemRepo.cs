using AutoMapper;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository
{
    public class CartItemRepo : ICartItemRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CartItemRepo(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public void AddItemToCar(CartItem cartItem)
        {
           _db.CartItems.Add(cartItem);
            _db.SaveChanges();
        }



        public async Task<CartItem> GetItemInCartAsync(int id)
        {
            var itemInCart = await _db.CartItems.FirstOrDefaultAsync(u => u.id == id );
            return itemInCart;
        }

        public void RemoveItemInCart(CartItem cartItem)
        {
            _db.CartItems.Remove(cartItem);
        }

        public void updateCartItem(CartItem cartItem)
        {

            _db.CartItems.Update(cartItem);
            _db.SaveChanges();
        }
    }
}
