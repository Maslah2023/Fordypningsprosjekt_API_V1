using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastFoodHouse_API.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<OrderRepo> _logger;

        public OrderRepo(ApplicationDbContext db, ILogger<OrderRepo> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<OrderHeader> CreateOrder(OrderHeader orderHeader, IEnumerable<OrderDetail> orderDetail)
        {
            try
            {
                if (orderHeader == null)
                {
                    throw new ArgumentNullException(nameof(orderHeader));
                }

                _db.OrderHeaders.Add(orderHeader);
                await _db.SaveChangesAsync();

                // Add order details if provided
                if (orderDetail != null)
                {
                    foreach (OrderDetail detail in orderDetail)
                    {
                        detail.OrderHeaderId = orderHeader.OrderheaderId;
                        _db.orderDetails.Add(detail);
                    }
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                // You may want to throw the exception here depending on your requirements
                return null;
            }

            return orderHeader;
        }

        public void DeleteOrderById(int id, int menuId)
        {
            try
            {
                OrderHeader orderToDelete = _db.OrderHeaders.Include(u => u.OrderDetails)
                    .FirstOrDefault(u => u.OrderheaderId == id);

                if (orderToDelete != null)
                {
                    if (menuId == 0 || orderToDelete.OrderDetails.Any(d => d.MenuItemId == menuId))
                    {
                        _db.OrderHeaders.Remove(orderToDelete);
                        _db.SaveChanges();
                    }
                    else
                    {
                        OrderDetail itemToRemove = orderToDelete.OrderDetails.FirstOrDefault(d => d.MenuItemId == menuId);
                        if (itemToRemove != null)
                        {
                            _db.orderDetails.Remove(itemToRemove);
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an order.");
                // You may want to throw the exception here depending on your requirements
            }
        }

        public async Task<OrderHeader> GetOrderById(int id)
        {
            try
            {
                OrderHeader orderHeader = await _db.OrderHeaders.Include(u => u.OrderDetails)
                    .FirstOrDefaultAsync(u => u.OrderheaderId == id);
                return orderHeader;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the order with ID: {id}.");
                return null;
            }
        }

        public async Task<IEnumerable<OrderHeader>> GetOrders(string userId)
        {
            try
            {
                IEnumerable<OrderHeader> orders = await _db.OrderHeaders.Include(u => u.OrderDetails).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders.");
                return null;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving changes.");
                // You may want to throw the exception here depending on your requirements
            }
        }

        public void UpdateOrderHeader(int id, OrderHeader order)
        {
            try
            {
                OrderHeader orderToUpdate = _db.OrderHeaders.AsNoTracking().SingleOrDefault(x => x.OrderheaderId == id);
                if (orderToUpdate != null)
                {
                    order.OrderheaderId = id;
                    _db.OrderHeaders.Update(order);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the order with ID: {id}.");
                // You may want to throw the exception here depending on your requirements
            }
        }
    }
}
