using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeRollBackService
{
    Task<RoomTypeResponse?> RollBackRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest);
}