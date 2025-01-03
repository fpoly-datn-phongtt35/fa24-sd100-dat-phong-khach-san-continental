using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;


namespace ViewClient.Repositories.IRepository
{
    public interface IRoombooking
    {
        Task<Guid> CreateRoomBooking(RoomBookingCreateRequestForCustomer request);
        Task<ResponseData<RoomBookingResponseForCustomer>> GetListRoomBookingByCustomerId(RoomBookingGetRequestByCustomer request);
    }
}
