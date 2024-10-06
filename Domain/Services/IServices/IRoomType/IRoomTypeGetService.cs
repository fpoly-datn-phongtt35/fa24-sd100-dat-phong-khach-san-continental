using Domain.DTO.RoomType;

namespace Domain.Services.IServices.IRoomType;

public interface IRoomTypeGetService
{
    Task<List<RoomTypeResponse>> GetAllRoomTypes(string? search);
    Task<RoomTypeResponse?> GetRoomTypeById(Guid? roomTypeId);
    Task<RoomTypeResponse?> GetRoomTypeWithAmenityRoomsAndRoomTypeServicesById(Guid roomTypeId);
}