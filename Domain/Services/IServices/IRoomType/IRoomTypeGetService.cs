using Domain.DTO.RoomType;
using Domain.Enums;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeGetService
{
    Task<List<RoomTypeResponse>> GetFilteredRoomTypes(string? searchString, EntityStatus? status);
    Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId);
    Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId);
    Task<List<RoomTypeResponse>> GetFilteredDeletedRoomTypes(string? searchString);
}