using Domain.DTO.Client;
using Domain.DTO.Customer;
using System.Data;

namespace ViewClient.Repositories.IRepository
{
    public interface ICustomer
    {
        Task<DataTable> GetCustomerById(Guid id);
        Task<ClientInsertCustomerViewModel> ClientInsertCustomer(ClientCreateCustomerRequest request);
    }
}
