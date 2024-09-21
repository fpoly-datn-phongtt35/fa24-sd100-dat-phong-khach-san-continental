using Domain.DTO.RoomType;

namespace Domain.Services.IServices.RoomType;

public interface IRoomTypeGetService
{
    Task<List<RoomTypeResponse>> GetAllRoomTypes();
    Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId);
}