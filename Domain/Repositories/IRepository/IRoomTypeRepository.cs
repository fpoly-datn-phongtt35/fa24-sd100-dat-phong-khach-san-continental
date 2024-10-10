using Domain.Enums;
using Domain.Models;

namespace Domain.Repositories.IRepository;

public interface IRoomTypeRepository
{
    public Task<List<RoomType>> GetFilteredRoomTypes(string? searchString, EntityStatus? status);
    public Task<RoomType?> GetRoomTypeById(Guid roomTypeId);
    public Task<RoomType?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId);
    public Task<RoomType> AddRoomType(RoomType roomType);
    public Task<RoomType?> UpdateRoomType(RoomType roomType);
    public Task<RoomType?> DeleteRoomType(RoomType roomType);
    public Task<List<RoomType>> GetFilteredDeletedRoomTypes(string? searchString);
    public Task<RoomType?> RecoverDeletedRoomType(RoomType roomType);
}