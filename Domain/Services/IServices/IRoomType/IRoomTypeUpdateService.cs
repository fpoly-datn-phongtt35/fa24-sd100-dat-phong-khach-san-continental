using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeUpdateService
{
    Task<RoomTypeResponse?> UpdateRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest);
    Task<RoomTypeResponse?> RecoverDeletedRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest);
}