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

        public CustomerService(ApplicationDbContext db, IAuthService authService, IMapper mapper, UserManager<Customer> userManager, ICustomerRepo customerRepo)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _customerRep = customerRepo;

        }

        public Task<Customer> DeleteCustomer(string customerId)
        {
            var customer = _customerRep.DeleteCustomer(customerId);
            return customer;
        }

        public Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customer = _customerRep.GetAllCustomers();
            return customer;
        }

        public Task<Customer> GetCustomerById(string customerId)
        {
            var customer = _customerRep.GetCustomerById(customerId);
            return customer;
        }

        public async Task<Customer> UpdateCustomer(string customerId, UpdateCustomerDTO updateCustomerDTO, string currentPassword, string newPassword)
        {
            var customerToUpdate = _mapper.Map<Customer>(updateCustomerDTO);
            var customer =  await _customerRep.UpdateCustomerById(customerId, customerToUpdate, currentPassword, newPassword);
            return customer;
           
            
        }

    }
}
