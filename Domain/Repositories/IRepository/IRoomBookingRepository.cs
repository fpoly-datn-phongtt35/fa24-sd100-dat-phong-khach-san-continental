using Domain.DTO.Customer;
using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Repositories.IRepository;

public interface IRoomBookingRepository
{
    Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBookings
        (RoomBookingGetRequest roomBookingGetRequest);
    Task<RoomBooking?> GetRoomBookingById(Guid roomBookingId);
    Task<RoomBooking?> UpdateRoomBooking(RoomBooking roomBooking);
    Task<Guid> CreateRoomBookingForCustomer(RoomBookingCreateRequestForCustomer request);
    Task<int> CreateRoomBooking(RoomBookingCreateRequest request);
    Task<int> UpdateRoomBookingStatus(Guid id, int status);
    Task<List<DateTimeOffset>> GetCheckinRoomBookingByRoomBookingId(Guid roomBookingId);
    Task<int> CheckDepositRoomBooking();
}