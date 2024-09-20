using Domain.DTO.Paging;
using Domain.DTO.ServiceOrder;
using Domain.DTO.ServiceType;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IServiceOrderService
    {
        Task<int> AddServiceOrder(ServiceOrderCreateRequest request);
        Task<ResponseData<ServiceOrder>> GetServiceOrders(ServiceOrderGetRequest request);
        Task<int> UpdateServiceOrder(ServiceOrderUpdateRequest request);
        Task<int> DeleteServiceOrder(ServiceOrderDeleteRequest request);
        Task<ServiceOrder> GetServiceOrderById(Guid Id);
    }
}
