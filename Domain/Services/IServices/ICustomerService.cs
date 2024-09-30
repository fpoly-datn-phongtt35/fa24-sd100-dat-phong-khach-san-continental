using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.ServiceType;
using Domain.Models;

namespace Domain.Services.IServices
{
    public interface ICustomerService
    {
        Task<int> AddCustomer(CustomerCreateRequest request);
        Task<int> UpdateCustomer(CustomerUpdateRequest request);
        Task<int> DeleteCustomer(CustomerDeleteRequest request);
        Task<ResponseData<Customer>> GetAllCustomer(CustomerGetByUserNameRequest customer);
        Task<Customer> GetCustomerById(Guid Id);
    }
}
