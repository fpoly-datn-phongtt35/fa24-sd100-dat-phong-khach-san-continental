using Domain.DTO.ServiceOrderDetail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepository
{
    public interface IServiceOrderDetailRepo
    {
        Task<int> AddServiceOrderDetail(ServiceOrderDetailCreateRequest request);
        Task<int> UpdateServiceOrderDetail(ServiceOrderDetailUpdateRequest request);
        Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request);
        Task<DataTable> GetServiceOrderDetails(ServiceOrderDetailGetRequest request);
    }
}
