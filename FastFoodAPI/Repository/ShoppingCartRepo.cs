using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Repository
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepo(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<ShoppingCart> CreateShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }

            _db.ShoppingCarts.Add(shoppingCart);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while creating shopping cart.", ex);
            }

            return shoppingCart;
        }

        public async Task<ShoppingCart> GetShoppingCart(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                return await _db.ShoppingCarts
                    .Include(u => u.CartItems)
                    .ThenInclude(u => u.MenuItem)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while retrieving shopping cart.", ex);
            }
        }

        public async Task<ShoppingCart> GetShoppingCartById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            try
            {
                return await _db.ShoppingCarts
                    .Include(u => u.CartItems)
                    .ThenInclude(u => u.MenuItem)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while retrieving shopping cart by ID.", ex);
            }
        }

        public void RemoveCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }

            try
            {
                _db.ShoppingCarts.Remove(shoppingCart);
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while removing shopping cart.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while saving changes asynchronously.", ex);
            }
        }

        public void UpdateShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }

            try
            {
                _db.ShoppingCarts.Update(shoppingCart);
                _db.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle it appropriately
                throw new Exception("Error occurred while updating shopping cart.", ex);
            }
        }
    }
}

