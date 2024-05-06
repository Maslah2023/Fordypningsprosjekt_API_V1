using AutoMapper;
using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using FastFoodHouse_API.Repository.Interface;
using FastFoodHouse_API.Service.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly ICustomerRepo _customerRep;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ApplicationDbContext db, IAuthService authService, IMapper mapper, UserManager<Customer> userManager, ICustomerRepo customerRepo, ILogger<CustomerService> logger)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _customerRep = customerRepo;
            _logger = logger;

        }

        public async Task<CustomerDTO> DeleteCustomer(string customerId)
        {
            try
            {
                var deletedCustomer = await _customerRep.DeleteCustomer(customerId);
                if (deletedCustomer == null)
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                    return null;
                }

                var deletedCustomerDTO = _mapper.Map<CustomerDTO>(deletedCustomer);
                return deletedCustomerDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID: {customerId}");
                return null;
            }
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerRep.GetAllCustomers();
                if (customers == null)
                {
                    _logger.LogWarning("No customers found");
                    return Enumerable.Empty<CustomerDTO>();
                }

                var customerDTOs = _mapper.Map<IEnumerable<CustomerDTO>>(customers);
                return customerDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all customers");
                return Enumerable.Empty<CustomerDTO>();
            }
        }

        public async Task<CustomerDTO> GetCustomerById(string customerId)
        {
            try
            {
                var customer = await _customerRep.GetCustomerById(customerId);
                if (customer == null)
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                    return null;
                }

                var customerDTO = _mapper.Map<CustomerDTO>(customer);
                return customerDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving customer with ID: {customerId}");
                return null;
            }
        }

        public async Task<CustomerDTO> UpdateCustomer(string customerId, UpdateCustomerDTO updateCustomerDTO, string currentPassword, string newPassword)
        {
            try
            {
                var customerToUpdate = _mapper.Map<Customer>(updateCustomerDTO);
                var updatedCustomer = await _customerRep.UpdateCustomerById(customerId, customerToUpdate, currentPassword, newPassword);
                if (updatedCustomer == null)
                {
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                    return null;
                }

                var updatedCustomerDTO = _mapper.Map<CustomerDTO>(updatedCustomer);
                return updatedCustomerDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating customer with ID: {customerId}");
                return null;
            }
        }

    }
}
