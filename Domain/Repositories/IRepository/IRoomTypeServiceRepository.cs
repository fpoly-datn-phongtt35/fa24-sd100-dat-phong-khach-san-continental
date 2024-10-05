using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeServiceRepository
{
    Task<List<RoomTypeService>> GetAllRoomTypeServices();
    Task<RoomTypeService?> GetRoomTypeServiceById(Guid roomTypeServiceId);
    Task<RoomTypeService> CreateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> UpdateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> DeleteRoomTypeService(RoomTypeService roomTypeService);
}