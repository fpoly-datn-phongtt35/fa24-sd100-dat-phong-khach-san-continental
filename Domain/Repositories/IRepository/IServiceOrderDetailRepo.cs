using Domain.DTO.ServiceOrderDetail;
using Domain.Models;
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
        Task<int> UpsertServiceOrderDetail(ServiceOrderDetail request);
        Task<DataTable> GetListServiceOrderDetailByRoomBookingDetailId(Guid id);
        Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request);
        Task<DataTable> GetServiceOrderDetails(ServiceOrderDetailGetRequest request);
    }
}
