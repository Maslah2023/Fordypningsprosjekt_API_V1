using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface ICustomerService
    {
        public Task<IEnumerable<Customer>> GetAllCustomers();
        public Task<Customer> GetCustomerById(string customerId);
        public Task<Customer> DeleteCustomer(string customrId);
        public Task<Customer> UpdateCustomer(string customerId, UpdateCustomerDTO user, string currentPassword, string newPassword);


    }
}
