using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface ICustomerService
    {
        Task<int> AddCustomer(CustomerCreateRequest request);
        Task<int> UpdateCustomer(CustomerUpdateRequest request);
        Task<int> DeleteCustomer(CustomerDeleteRequest request);
        Task<Customer> GetCustomerById(Guid Id);
    }
}
