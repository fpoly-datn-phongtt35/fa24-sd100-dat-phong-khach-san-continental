using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeGetService
{
    Task<List<RoomTypeResponse>> GetAllRoomTypes();
    Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId);
}