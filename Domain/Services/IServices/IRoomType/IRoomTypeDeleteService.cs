using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeDeleteService
{
    Task<RoomTypeResponse?> DeleteRoomType(RoomTypeDeleteRequest roomTypeDeleteRequest);
}