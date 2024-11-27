using Domain.DTO.RoomBooking;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomBooking;

namespace Domain.Services.Services.RoomBooking;

public class RoomBookingUpdateService : IRoomBookingUpdateService
{
    private readonly IRoomBookingRepository _roomBookingRepository;

    public RoomBookingUpdateService(IRoomBookingRepository roomBookingRepository)
    {
        _roomBookingRepository = roomBookingRepository;
    }

    public async Task<RoomBookingResponse?> UpdateRoomBookingAsync(RoomBookingUpdateRequest roomBookingUpdateRequest)
    {
        if(roomBookingUpdateRequest is null)
            throw new ArgumentNullException(nameof(roomBookingUpdateRequest));
        
        var existingRoomBooking = await _roomBookingRepository
            .GetRoomBookingById(roomBookingUpdateRequest.Id);
        if(existingRoomBooking is null)
            throw new Exception("Room booking not found");
        
        existingRoomBooking.Status = roomBookingUpdateRequest.Status;
        existingRoomBooking.ModifiedTime = roomBookingUpdateRequest.ModifiedTime;
        existingRoomBooking.ModifiedBy = roomBookingUpdateRequest.ModifiedBy;
        
        await _roomBookingRepository.UpdateRoomBooking(existingRoomBooking);
        return existingRoomBooking.ToRoomBookingResponse();
    }
}