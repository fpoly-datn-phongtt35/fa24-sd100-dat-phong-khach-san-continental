using Domain.DTO.RoomBooking;
using Domain.Enums;
using Domain.Models;

namespace Domain.Services.IServices.IRoomBooking;

public interface IRoomBookingUpdateService
{
    Task<RoomBookingResponse?> UpdateRoomBookingAsync(RoomBookingUpdateRequest roomBookingUpdateRequest);
    Task<int> UpdateRoomBookingStatus(Guid id, int status);
}