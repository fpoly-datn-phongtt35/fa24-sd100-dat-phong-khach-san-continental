using Domain.DTO.ServiceOrder;
using Domain.DTO.ServiceType;
using Domain.Models;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IServiceOrderRepo
    {
        Task<int> AddServiceOrder(ServiceOrderCreateRequest request);
        Task<DataTable> GetServiceOrders(ServiceOrderGetRequest request);
        Task<int> UpdateServiceOrder(ServiceOrderUpdateRequest request);
        Task<int> DeleteServiceOrder(ServiceOrderDeleteRequest request);
        Task<DataTable> GetServiceOrderById(Guid Id);
    }
}
