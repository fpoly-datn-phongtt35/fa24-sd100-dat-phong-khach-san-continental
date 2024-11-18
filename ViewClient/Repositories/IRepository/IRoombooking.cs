using Domain.DTO.RoomBooking;

namespace ViewClient.Repositories.IRepository
{
    public interface IRoombooking
    {
        Task<int> CreateRoomBooking(RoomBookingCreateRequest request);
    }
}
