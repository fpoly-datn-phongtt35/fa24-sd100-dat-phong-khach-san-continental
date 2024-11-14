using Domain.DTO.RoomBookingDetail;

namespace Domain.Repositories.IRepository
{
    public interface IRoomBookingDetailRepository
    {
        Task<int> CreateRoomBookingDetailForCustomer(RoomBookingDetailCreateRequestForCustomer request);
    }
}
