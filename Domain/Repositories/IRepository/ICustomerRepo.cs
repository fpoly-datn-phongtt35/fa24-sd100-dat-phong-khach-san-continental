using Domain.DTO.Customer;
using Domain.DTO.Service;
using System.Data;

namespace Domain.Repositories.IRepository
{
    interface ICustomerRepo
    {
        Task<int> AddCustomer(CustomerCreateRequest request);
        Task<int> UpdateCustomer(CustomerUpdateRequest request);
        Task<int> DeleteCustomer(CustomerDeleteRequest request);
        
    }
}
