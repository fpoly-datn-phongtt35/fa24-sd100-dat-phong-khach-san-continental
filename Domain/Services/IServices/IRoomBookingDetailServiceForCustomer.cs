using Domain.DTO.RoomBookingDetail;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IRoomBookingDetailServiceForCustomer
    {
        Task<List<RoomBookingDetailGetByIdRoomBooking>> GetListRoomBookingDetailByRoomBookingId(Guid id);
        Task<RoomBookingDetail> GetById(Guid id);
        Task<RoomBookingDetailResponse?> GetRoomBookingDetailById2(Guid id);
        Task<RoomBookingDetailResponse?> UpdateRoomBookingDetail2
            (RoomBookingDetailUpdateRequest roomBookingDetailUpdateRequest);
        Task<int> UpSertRoomBookingDetail(RoomBookingDetail request);
        Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
        Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId);
    }
}
