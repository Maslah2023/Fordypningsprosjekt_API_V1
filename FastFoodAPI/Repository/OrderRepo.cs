using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _db;

        public OrderRepo(ApplicationDbContext db)
        {
            _db = db;
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
                 _db.SaveChanges();

                if (orderDetail == null)
                {
                    throw new ArgumentNullException();
                }
                foreach (OrderDetail detail in orderDetail)
                {
                    _db.orderDetails.Add(detail);
                }
                _db.SaveChanges();


            }
            catch 
            {
                Console.WriteLine($"An error occured while retrieving");
                return null;
           
            }

            return orderHeader;






        }

        public async Task<OrderHeader> GetOrderById(int id)
        {
            OrderHeader? orderHeader = 
            await _db.OrderHeaders.FindAsync(id);
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
                IEnumerable<OrderHeader> orders = await _db.OrderHeaders.ToListAsync();
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
