using FastFoodHouse_API.Data;
using FastFoodHouse_API.Models;
using FastFoodHouse_API.Models.Dtos;

namespace FastFoodHouse_API.Service.Interface
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerDTO>> GetAllCustomers();
        public Task<CustomerDTO> GetCustomerById(string customerId);
        public Task<CustomerDTO> DeleteCustomer(string customrId);
        public Task<CustomerDTO> UpdateCustomer(string customerId, UpdateCustomerDTO user, string currentPassword, string newPassword);


    }
}
