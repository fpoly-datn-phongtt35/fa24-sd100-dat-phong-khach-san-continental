using Domain.DTO.Client;
using Domain.DTO.Customer;
using System.Data;

namespace ViewClient.Repositories.IRepository
{
    public interface ICustomer
    {
        Task<CustomerGetByIdRequest> GetCustomerById(Guid id);
        Task<ClientInsertCustomerViewModel> ClientInsertCustomer(ClientCreateCustomerRequest request);
        Task<int> UpdateCustomer(CustomerUpdateRequest request);
        Task<DataTable> ClientUpdatePassword(ClientUpdatePassword request);
    }
}
