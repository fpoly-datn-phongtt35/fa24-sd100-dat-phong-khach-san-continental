using Domain.DTO.RoomType;

namespace Domain.Services.IServices.RoomType;

public interface IRoomTypeUpdateService
{
    Task<RoomTypeResponse?> UpdateRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest);
}