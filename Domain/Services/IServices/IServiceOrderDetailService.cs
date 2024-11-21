using Domain.DTO.Paging;
using Domain.DTO.ServiceOrderDetail;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IServiceOrderDetailService
    {
        Task<int> UpsertServiceOrderDetail(ServiceOrderDetail request);
        Task<List<ServiceOrderDetailResponse>> GetListServiceOrderDetailByRoomBookingI(Guid id);
        Task<int> DeleteServiceOrderDetail(ServiceOrderDetailDeleteRequest request);
        Task<ServiceOrderDetail> GetServiceOrderDetailById(Guid Id);
        Task<ResponseData<ServiceOrderDetail>> GetServiceOrderDetails(ServiceOrderDetailGetRequest request);
        Task<ResponseData<ServiceOrderDetail>> GetServiceOrderDetailByRoomBookingId(Guid id);
    }
}
