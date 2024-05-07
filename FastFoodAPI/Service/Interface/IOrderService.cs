using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface IOrderService 
    {
        Task<OrderHeaderDTO> GetOrderById(int id);
        Task<IEnumerable<OrderHeaderDTO>> GetOrders();
        Task<OrderHeaderDTO> CreateOrder(OrderHeaderDTO order);
        void UpdateOrder(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO);
        void DeleteOrderById(int id, int menuId);   
    }
}
