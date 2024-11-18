using System.Data;

namespace ViewClient.Repositories.IRepository
{
    public interface ICustomer
    {
        Task<DataTable> GetCustomerById(Guid id);
    }
}
