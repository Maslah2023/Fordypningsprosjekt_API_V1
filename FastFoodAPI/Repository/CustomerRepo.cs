using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomerRepo(ApplicationDbContext db, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //public  void DeleteCustomerB(string customrId)
        //{
        //    var customerToDelete = _db.Customers.FirstOrDefault(x => x.Id == customrId);
        //    _db.Customers.Remove(customerToDelete);

        //}
        

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            try
            {
                var users = await
               _db.Customers.ToListAsync();
                if (users == null)
                {
                    return null;
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving customers: {ex.Message}");
                return null;
            }


        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            try
            {
                Customer? customer = await
               _db.Customers.SingleOrDefaultAsync(x => x.Id == customerId);
                if (customer == null)
                {
                    return null;
                }
                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving customer with  id : {ex.Message}");
                return null;
            }
        }

        public async Task<Customer> UpdateCustomerById(string customerId,Customer user)
        {
            try
            {
                
                var customer = await
               _db.Customers.FirstOrDefaultAsync(u => u.Id == customerId);
                customer.Id = user.Id;
                customer.UserName = user.UserName;
                customer.Email = user.Email;
                customer.PhoneNumber = user.PhoneNumber;

                _db.Customers.Update(customer);
                await _db.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while updating: {ex.Message}");
                return null;
            }


        }




        public async Task<Customer> DeleteCustomer(string customerId)
        {
            try
            {
            var customer = _db.Customers.SingleOrDefault(x => x.Id == customerId);
                if (customer == null)
                {
                    return null;
                }
                _db.Customers.Remove(customer);
                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving");
                return null ;
            }

        }



    }
}
