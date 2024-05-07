using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface IOrderRepo
    {
        Task<IEnumerable<OrderHeader>> GetOrders();
        Task<OrderHeader> GetOrderById(int id);
        Task<OrderHeader> CreateOrder(OrderHeader order, IEnumerable<OrderDetail> orderDetail);
        void UpdateOrderHeader(int id, OrderHeader order);
        void DeleteOrderById(int id, int menuId);   
    }
}
