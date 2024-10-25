using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.Enums;

namespace Domain.Services.IServices.IAmenityRoom;

public interface IAmenityRoomGetService
{
    Task<ResponseData<AmenityRoomResponse>> GetFilteredAmenityRooms(AmenityRoomGetRequest amenityRoomGetRequest);
    Task<ResponseData<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms(AmenityRoomGetRequest amenityRoomGetRequest);
    Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId);
}