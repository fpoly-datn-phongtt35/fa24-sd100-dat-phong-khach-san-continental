using Domain.DTO.RoomBookingDetail;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoomBookingDetail
    {
        Task<int> CreateRoomBookingDetail(RoomBookingDetailCreateRequest request);
        Task<int> UpdateRoomBookingDetail(RoomBookingDetailUpdateRequest request);
    }
}
