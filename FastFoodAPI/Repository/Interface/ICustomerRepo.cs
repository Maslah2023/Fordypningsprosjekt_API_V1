using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastFoodHouse_API.Repository.Interface
{
    public interface ICustomerRepo
    {
        public Task<IEnumerable<Customer>> GetAllCustomers();
        public Task<Customer> GetCustomerById(string customerId);
        public  Task<Customer> DeleteCustomer(string customrId);
        public Task<Customer> UpdateCustomerById(string customerId, Customer user, string currentPassword, string newPassword);

        void UpdateAsync(Customer customer);

    }
}
