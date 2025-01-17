using Domain.DTO.RoomBookingDetail;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoomBookingDetail
    {
        Task<Guid> CreateRoomBookingDetail(RoomBookingDetailCreateRequestForCustomer request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
    }
}
