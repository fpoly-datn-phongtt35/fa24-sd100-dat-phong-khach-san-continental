using Domain.DTO.Paging;
using Domain.DTO.RoomBooking;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomBooking;

namespace Domain.Services.Services.RoomBooking;

public class RoomBookingGetService : IRoomBookingGetService
{
    private readonly IRoomBookingRepository _roomBookingRepository;

    public RoomBookingGetService(IRoomBookingRepository roomBookingRepository)
    {
        _roomBookingRepository = roomBookingRepository;
    }

    public async Task<ResponseData<RoomBookingResponse>> GetFilteredRoomBooking(RoomBookingGetRequest bookingGetRequest)
    {
        return await _roomBookingRepository.GetFilteredRoomBookings(bookingGetRequest);
    }

    public async Task<RoomBookingResponse?> GetRoomBookingById(Guid? roomBookingId)
    {
        if(roomBookingId == null) return null;
        var roomBooking = await _roomBookingRepository.GetRoomBookingById(roomBookingId.Value);
        if(roomBooking == null) return null;

        return roomBooking.ToRoomBookingResponse();
    }
}