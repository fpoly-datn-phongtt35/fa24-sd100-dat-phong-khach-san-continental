using Domain.DTO.RoomBooking;
using Domain.Models;

namespace Domain.Services.IServices.IRoomBooking;

public interface IRoomBookingUpdateService
{
    Task<RoomBookingResponse?> UpdateRoomBookingAsync(RoomBookingUpdateRequest roomBookingUpdateRequest);
}