using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.Models;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Domain.Repositories.IRepository;

public interface IAmenityRoomRepository
{
    Task<ResponseData<AmenityRoomResponse>> GetFilteredAmenityRooms(AmenityRoomGetRequest amenityRoomGetRequest);
    Task<ResponseData<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms
        (AmenityRoomGetRequest amenityRoomGetRequest);
    Task<AmenityRoom?> GetAmenityRoomById(Guid amenityRoomId);
    Task<AmenityRoom> AddAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> UpdateAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> DeleteAmenityRoom(AmenityRoom amenityRoom);
    Task<AmenityRoom?> RecoverDeletedAmenityRoom(AmenityRoom amenityRoom);
}