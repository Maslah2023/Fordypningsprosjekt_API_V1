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

        public async Task<CustomerDTO> DeleteCustomer(string customerId)
        {
           CustomerDTO customerDTO =
           _mapper.Map<CustomerDTO>
           (await _customerRep
           .DeleteCustomer(customerId));
           return customerDTO;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customer = await _customerRep.GetAllCustomers();
            IEnumerable<CustomerDTO> user = _mapper.Map<IEnumerable<CustomerDTO>>(customer);
            return user;
        }

        public async Task<CustomerDTO> GetCustomerById(string customerId)
        {
            CustomerDTO customer =
            _mapper.Map<CustomerDTO>
           (await _customerRep.GetCustomerById(customerId));
            return customer;
        }

        public async Task<CustomerDTO> UpdateCustomer(string customerId, UpdateCustomerDTO updateCustomerDTO, string currentPassword, string newPassword)
        {
            var customerToUpdate = _mapper.Map<Customer>(updateCustomerDTO);
            var customer =  await _customerRep.UpdateCustomerById(customerId, customerToUpdate, currentPassword, newPassword);
            CustomerDTO customerDTO = _mapper.Map<CustomerDTO>(customer);
            return customerDTO;
           
            
        }

    }
}
