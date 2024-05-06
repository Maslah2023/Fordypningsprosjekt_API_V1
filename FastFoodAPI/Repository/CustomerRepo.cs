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
                
                var customers = await _db.Customers.ToListAsync();
                if (customers.Count() == 0)
                {
                    // Log a warning if users is null (unlikely if there's an error in fetching)
                    _logger.LogWarning("No customers found in the database");
                    return Enumerable.Empty<Customer>(); // Return an empty collection
                }
                return customers;
            }
            catch (Exception ex)
            {
                // Log the error
                _logger.LogError(ex, "An error occurred while retrieving customers");

                // Handle the error locally (e.g., return a default value or an empty collection)
                return Enumerable.Empty<Customer>(); // Return an empty collection
            }

        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            try
            {
                Customer customer = await _db.Customers.SingleOrDefaultAsync(x => x.Id == customerId);
                if (customer == null)
                {
                    return null;
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving customer with ID: {customerId}");
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
                    customer.Email = user.UserName;
                    customer.Email = user.Email;
                    customer.NormalizedEmail = user.Email;
                    customer.Address = user.Address;
                    customer.PhoneNumber = user.PhoneNumber;
                    customer.City = user.City;
       
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
                    // Log a warning if customer is null (unlikely if there's an error in fetching)
                    _logger.LogWarning("Customer not found with ID: {CustomerId}", customerId);
                    return null; // or throw new NotFoundException("Customer not found") depending on your requirements
                }

                await _userManager.DeleteAsync(customer);
                await _db.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting customer with ID: {customerId}");
                return null; // or throw new DeleteCustomerException("Failed to delete customer", ex) depending on your requirements
            }

        }

        public void UpdateAsync(Customer customer)
        {
            _userManager.UpdateAsync(customer);
        }
    }
}
