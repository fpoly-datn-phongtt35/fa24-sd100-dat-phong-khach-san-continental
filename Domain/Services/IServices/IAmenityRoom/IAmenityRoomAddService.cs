using Domain.DTO.AmenityRoom;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomAddService
{
    Task<AmenityRoomResponse> AddAmenityRoomService(AmenityRoomAddRequest amenityRoomAddRequest);
}