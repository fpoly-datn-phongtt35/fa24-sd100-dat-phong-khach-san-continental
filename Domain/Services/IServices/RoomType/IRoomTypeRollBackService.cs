using Domain.DTO.RoomType;

namespace Domain.Services.IServices.RoomType;

public interface IRoomTypeRollBackService
{
    Task<RoomTypeResponse?> RollBackRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest);
}