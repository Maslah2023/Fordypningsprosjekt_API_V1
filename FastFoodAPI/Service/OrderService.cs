using AutoMapper;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;

namespace FastFoodHouse_API.Service
{
    public class OrderService : IOrderService

    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepo orderRepo, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<OrderHeaderDTO> CreateOrder(OrderHeaderDTO orderHeaderDTO)
        {
            try
            {
                //OrderHeader newOrder = new OrderHeader()
                //{
                //    ApplicationUserId = orderHeaderDTO.ApplicationUserId,
                //    PickupName = orderHeaderDTO.PickupName,
                //    PickUpPhoneNumber = orderHeaderDTO.PickUpPhoneNumber,
                //    PickupEmail = orderHeaderDTO.PickupEmail,
                //    OrderDate = DateTime.Now,
                //};
                OrderHeader? orderHeader = _mapper.Map<OrderHeader>(orderHeaderDTO);
                //List<OrderDetail> orderDetails = _mapper.Map<List<OrderDetail>>(orderHeaderDTO.OrderDetails);
             
                var order  = await _orderRepo.CreateOrder(orderHeader, orderHeader.OrderDetails);
                if (order != null)
                {
                    return null;
                }
                OrderHeaderDTO orderDTO = _mapper.Map<OrderHeaderDTO>(order);
                return orderDTO;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public void DeleteOrderById(int id, int menuId)
        {
            _orderRepo.DeleteOrderById(id, menuId);
        }

        public async Task<OrderHeaderDTO> GetOrderById(int id)
        {
            try
            {
                OrderHeader? order = await 
                _orderRepo.GetOrderById(id);
                if(order == null) 
                {
                    return null;
                }
                OrderHeaderDTO orderDTO = _mapper.Map<OrderHeaderDTO>(order);
                return orderDTO;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message, "An error occured in the GetOrderById Method at Service layer ");
                throw;
            }
        }

        public async Task<IEnumerable<OrderHeaderDTO>> GetOrders(string userId)
        {
            try
            {
                IEnumerable<OrderHeader> orders = await _orderRepo.GetOrders(userId);
                IEnumerable<OrderHeaderDTO> orderHeaderDTO = _mapper.Map<IEnumerable<OrderHeaderDTO>>(orders);
                return orderHeaderDTO;
                

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in the GetOrders method.");
                throw;
            }
         

        }

        public void UpdateOrder(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderHeaderUpdateDTO);
                _orderRepo.UpdateOrderHeader(id, orderHeader);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occured in the UpdateOrder method at service layer");
            }
           
        }
    }
}
