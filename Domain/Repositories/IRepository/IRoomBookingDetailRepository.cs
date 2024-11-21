using Domain.DTO.RoomBookingDetail;
using System.Data;

namespace Domain.Repositories.IRepository
{
    public interface IRoomBookingDetailRepository
    {
        Task<DataTable> GetListRoomBookingDetailByRoomBookingId(Guid id);
        Task<DataTable> GetById(Guid id);
        Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
        Task<DataTable> GetRoomBookingDetailByCustomerId(Guid customerId);
    }
}
