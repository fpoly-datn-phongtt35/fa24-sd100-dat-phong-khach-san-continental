using Domain.DTO.AmenityRoom;
using Domain.Enums;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomGetService
{
    Task<List<AmenityRoomResponse>> GetFilteredAmenityRooms(string? searchString, 
        Guid? roomTypeId, EntityStatus? status);
    Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId);
    Task<List<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms(string? searchString, Guid? roomTypeId);
}