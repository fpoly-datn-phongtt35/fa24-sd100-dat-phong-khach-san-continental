using Domain.DTO.AmenityRoom;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomUpdateService
{
    Task<AmenityRoomResponse?> UpdateAmenityRoom(AmenityRoomUpdateRequest amenityRoomUpdateRequest);
    Task<AmenityRoomResponse?> RecoverDeletedAmenityRoom(AmenityRoomUpdateRequest amenityRoomUpdateRequest);
}