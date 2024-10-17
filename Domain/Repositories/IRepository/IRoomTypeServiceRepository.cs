using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeServiceRepository
{
    Task<List<RoomTypeService>> GetFilteredRoomTypeServices(string? searchString, Guid? roomTypeId,
        EntityStatus? status);
    Task<RoomTypeService?> GetRoomTypeServiceById(Guid roomTypeServiceId);
    Task<RoomTypeService> CreateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> UpdateRoomTypeService(RoomTypeService roomTypeService);
    Task<RoomTypeService?> DeleteRoomTypeService(RoomTypeService roomTypeService);
    Task<List<RoomTypeService>> GetFilteredDeletedRoomTypeServices (string? searchString, Guid? roomTypeId);
    Task<RoomTypeService?> RecoverDeletedRoomTypeService(RoomTypeService roomTypeService);
}