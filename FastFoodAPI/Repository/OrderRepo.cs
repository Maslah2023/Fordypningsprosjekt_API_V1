using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;

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
                    throw new ArgumentNullException();
                }
                _db.OrderHeaders.Add(orderHeader);
                await _db.SaveChangesAsync();

                //if (orderDetail == null)
                //{
                //    throw new ArgumentNullException();
                //}
                //foreach (OrderDetail detail in orderDetail)
                //{
                //    detail.OrderHeaderId = orderHeader.OrderheaderId;
                //    _db.orderDetails.Add(detail);
                //}
                //await _db.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving");
                return null;

            }

            return orderHeader;






        }

        public void DeleteOrderById(int id, int menuId)
        {
            try
            {
                OrderHeader? orderToDelete = _db.OrderHeaders.Include(u => u.OrderDetails)
                .FirstOrDefault(u => u.OrderheaderId == id);
                OrderDetail itemToRemove = _db.orderDetails.FirstOrDefault(u => u.MenuItemId == menuId);

                if (menuId == 0)
                {
                    _db.Remove(orderToDelete);
                    _db.SaveChanges();
                }
                else if(itemToRemove.MenuItemId == menuId && orderToDelete.OrderDetails.Count() != 1) 
                {
                    _db.orderDetails.Remove(itemToRemove);
                    _db.SaveChanges();
                } 
                else
                {
                    _db.OrderHeaders.Remove(orderToDelete);
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
          
            
           

        
           
        }

        public async Task<OrderHeader> GetOrderById(int id)
        {
            OrderHeader? orderHeader =
            await _db.OrderHeaders.Include(u => u.OrderDetails).FirstOrDefaultAsync(u => u.OrderheaderId == id);
            if (orderHeader == null)
            {
                return null;
            }
            return orderHeader;
        }

        public async Task<IEnumerable<OrderHeader>> GetOrders(string userId)
        {
            try
            {
                IEnumerable<OrderHeader> orders = await
              _db.OrderHeaders.Include(u => u.OrderDetails).ToListAsync();
                return orders;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async void SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void UpdateOrderHeader(int id, OrderHeader order)
        {
            OrderHeader? orderToUpdate = _db.OrderHeaders.AsNoTracking().SingleOrDefault(x => x.OrderheaderId == id);
            order.OrderheaderId = id;
            _db.OrderHeaders.Update(order);
            _db.SaveChanges();
        }
    }
}
