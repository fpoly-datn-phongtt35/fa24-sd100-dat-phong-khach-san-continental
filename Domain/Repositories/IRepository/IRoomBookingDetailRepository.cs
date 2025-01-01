using Domain.DTO.RoomBookingDetail;
using Domain.Models;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IRoomBookingDetailRepository
    {
        Task<DataTable> GetListRoomBookingDetailByRoomBookingId(Guid id);
        Task<DataTable> GetById(Guid id);
        Task<DataTable> GetRoomBookingDetailById2(Guid id);
        Task<int> UpSertRoomBookingDetail(RoomBookingDetail request);
        Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
        Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId);
    }
}
