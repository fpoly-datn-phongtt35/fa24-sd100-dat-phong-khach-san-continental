using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Services.IServices.IRoomBooking;

public interface IRoomBookingGetService
{
    Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBooking
        (RoomBookingGetRequest bookingGetRequest);
    Task<RoomBookingResponse?> GetRoomBookingById(Guid? roomBookingId);
}