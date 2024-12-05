using Domain.DTO.Customer;
using System.Data;

namespace ViewClient.Repositories.IRepository
{
    public interface ICustomer
    {
        Task<DataTable> GetCustomerById(Guid id);
        Task<DataTable> ClientInsertCustomer(ClientCreateCustomerRequest request);
    }
}
