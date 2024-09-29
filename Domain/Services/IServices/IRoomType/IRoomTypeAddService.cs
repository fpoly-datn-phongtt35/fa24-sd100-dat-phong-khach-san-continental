using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeAddService
{
    Task<RoomTypeResponse> AddRoomType(RoomTypeAddRequest roomTypeAddRequest);
}