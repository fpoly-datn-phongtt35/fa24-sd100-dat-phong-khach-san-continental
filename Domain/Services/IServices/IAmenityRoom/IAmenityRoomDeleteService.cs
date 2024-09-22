using Domain.DTO.AmenityRoom;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomDeleteService
{
    Task<AmenityRoomResponse?> DeleteAmenityRoom(AmenityRoomDeleteRequest amenityRoomDeleteRequest);
}