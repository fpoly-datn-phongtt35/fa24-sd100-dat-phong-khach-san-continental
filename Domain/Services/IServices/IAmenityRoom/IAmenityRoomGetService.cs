using Domain.DTO.AmenityRoom;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomGetService
{
    Task<List<AmenityRoomResponse>> GetAllAmenityRooms(string? search);
    Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId);
}