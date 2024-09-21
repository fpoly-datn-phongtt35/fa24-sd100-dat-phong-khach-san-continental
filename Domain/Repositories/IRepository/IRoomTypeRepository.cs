using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeRepository
{
    public Task<List<RoomType>> GetAllRoomTypes();
    public Task<RoomType?> GetRoomTypeById(Guid roomTypeId);
    public Task<RoomType> AddRoomType(RoomType roomType);
    public Task<RoomType?> UpdateRoomType(RoomType roomType);
    public Task<RoomType?> DeleteRoomType(RoomType roomType);
    public Task<RoomType?> RollBackDeleteRoomType(RoomType roomType);
}