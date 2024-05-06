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
        private readonly ILogger<CustomerRepo> _logger;

        public CustomerRepo(ApplicationDbContext db, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, ILogger<CustomerRepo> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
     
        

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

        public async Task<Customer> UpdateCustomerById(string customerId, Customer user, string currentPassword, string newPassword)
        {
            try
            {
                Customer customer = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == customerId);
                //IdentityResult result;
                if (customer != null)
                {
                    customer.Name = user.Name;
                    customer.UserName = user.UserName;
                    customer.NormalizedUserName = user.UserName;
       
                }
                IdentityResult result = await _userManager.UpdateAsync(customer);
                if (result.Succeeded)
                {
                    bool isPasswordValid = await _userManager.CheckPasswordAsync(customer, currentPassword);
                    if (isPasswordValid)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(customer, currentPassword, newPassword);
                        if (changePasswordResult.Succeeded)
                        {
                            await _db.SaveChangesAsync();
                            return customer;
                        }
                    }
                   
                }
                else
                {
                    // Customer not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(($"An error occurred while updating: {ex.Message}"));
            }
            return null;

         }




        public async Task<Customer> DeleteCustomer(string customerId)
        {
            try
            {
            var customer = await _userManager.FindByIdAsync(customerId);
                if (customer == null)
                {
                    return null;
                }
               await  _userManager.DeleteAsync(customer);
               await _db.SaveChangesAsync();
               return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while retrieving");
                return null ;
            }

        }

        public void UpdateAsync(Customer customer)
        {
            _userManager.UpdateAsync(customer);
        }
    }
}
