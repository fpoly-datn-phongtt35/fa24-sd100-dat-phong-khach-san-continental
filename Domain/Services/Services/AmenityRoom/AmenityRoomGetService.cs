using Domain.DTO.AmenityRoom;
using Domain.DTO.Paging;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenityRoom;

namespace Domain.Services.Services.AmenityRoom;

public class AmenityRoomGetService : IAmenityRoomGetService
{
    private readonly IAmenityRoomRepository _amenityRoomRepository;

    public AmenityRoomGetService(IAmenityRoomRepository amenityRoomRepository)
    {
        _amenityRoomRepository = amenityRoomRepository;
    }

    public async Task<ResponseData<AmenityRoomResponse>> GetFilteredAmenityRooms
        (AmenityRoomGetRequest amenityRoomGetRequest)
    {
        return await _amenityRoomRepository.GetFilteredAmenityRooms(amenityRoomGetRequest);
    }
    
    public async Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId)
    {
        if (amenityRoomId == null) return null;
        
        var amenityRoom = await _amenityRoomRepository.GetAmenityRoomById(amenityRoomId.Value);
        if (amenityRoom == null) return null;

        return amenityRoom.ToAmenityRoomResponse();
    }

    public async Task<ResponseData<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms
        (AmenityRoomGetRequest amenityRoomGetRequest)
    {
        return await _amenityRoomRepository.GetFilteredDeletedAmenityRooms(amenityRoomGetRequest);
    }
}