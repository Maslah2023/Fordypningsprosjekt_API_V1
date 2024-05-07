using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderHeaderDTO);
                var order = await _orderRepo.CreateOrder(orderHeader, orderHeader.OrderDetails);
                if (order == null)
                {
                    // Handle error here, depending on your requirements
                    _logger.LogError("Failed to create order.");
                    return null;
                }
                OrderHeaderDTO orderDTO = _mapper.Map<OrderHeaderDTO>(order);
                return orderDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating an order.");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        public void DeleteOrderById(int id, int menuId)
        {
            try
            {
                _orderRepo.DeleteOrderById(id, menuId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an order.");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        public async Task<OrderHeaderDTO> GetOrderById(int id)
        {
            try
            {
                OrderHeader order = await _orderRepo.GetOrderById(id);
                if (order == null)
                {
                    // Handle error here, depending on your requirements
                    _logger.LogError($"Order with ID {id} not found.");
                    return null;
                }
                OrderHeaderDTO orderDTO = _mapper.Map<OrderHeaderDTO>(order);
                return orderDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving order with ID {id}.");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        public async Task<IEnumerable<OrderHeaderDTO>> GetOrders()
        {
            try
            {
                IEnumerable<OrderHeader> orders = await _orderRepo.GetOrders();
                IEnumerable<OrderHeaderDTO> orderHeaderDTO = _mapper.Map<IEnumerable<OrderHeaderDTO>>(orders);
                return orderHeaderDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving orders.");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }

        public void UpdateOrder(int id, OrderHeaderUpdateDTO orderHeaderUpdateDTO)
        {
            try
            {
                OrderHeader orderHeader = _mapper.Map<OrderHeader>(orderHeaderUpdateDTO);
                _orderRepo.UpdateOrderHeader(id, orderHeader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating order with ID {id}.");
                throw; // Re-throw the exception to propagate it to the caller
            }
        }
    }
}

