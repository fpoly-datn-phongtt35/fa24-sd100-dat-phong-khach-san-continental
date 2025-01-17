using Domain.DTO.RoomBookingDetail;
using Domain.Models;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IRoomBookingDetailRepository
    {
        Task<DataTable> GetListRoomBookingDetailByRoomBookingId(Guid id);
        Task<DataTable> GetById(Guid id);
        Task<RoomBookingDetail?> GetRoomBookingDetailById2(Guid id);
        Task<RoomBookingDetail?> GetRoomBookingDetailWithEditHistory(Guid roomBookingDetailId);
        Task<RoomBookingDetail?> UpdateRoomBookingDetail2(RoomBookingDetail roomBookingDetail);
        Task<int> UpSertRoomBookingDetail(RoomBookingDetail request);
        Task<Guid> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
        Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId);
    }
}
