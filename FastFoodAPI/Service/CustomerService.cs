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

        public void DeleteCustomer(string customerId)
        {
            var customer = _customerRep.DeleteCustomer(customerId);
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

        public void  UpdateCustomer(string customerId,UpdateCustomerDTO updateCustomerDTO)
        {
            var customerToUpdate = _mapper.Map<Customer>(updateCustomerDTO);
            _customerRep.UpdateCustomerById(customerId, customerToUpdate);
           
            
        }
        //public async Task AddUser()
        //{
        //    IEnumerable<Customer> listUsers = _mapper.Map<IEnumerable<Customer>>(await _authService.GetAllUsers());
        //    foreach (var user in listUsers)
        //    {
        //        _db.Customer.Add(user);
        //        await _db.SaveChangesAsync();
        //    }

        //}

        //public async Task UpdateCustomer(string id, UpdateCustomerDTO updateCustomerDTO)
        //{
        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        //    user.Name = updateCustomerDTO.Name;
        //    IdentityResult x = await _userManager.UpdateAsync(user);
        //    if (x != null)
        //    {

        //    }
        //}




    }
}
