using Domain.DTO.RoomType;

namespace Domain.Services.IServices.RoomType;

public interface IRoomTypeDeleteService
{
    Task<RoomTypeResponse?> DeleteRoomType(RoomTypeDeleteRequest roomTypeDeleteRequest);
}