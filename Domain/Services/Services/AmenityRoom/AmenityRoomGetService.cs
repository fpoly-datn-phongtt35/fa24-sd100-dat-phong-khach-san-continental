using Domain.DTO.AmenityRoom;
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

    public async Task<List<AmenityRoomResponse>> GetFilteredAmenityRooms(string? searchString,
        Guid? roomTypeId, EntityStatus? status)
    {
        var amenityRooms = await _amenityRoomRepository
            .GetFilteredAmenityRooms(searchString, roomTypeId, status);

        var amenityRoomsResponse = amenityRooms
            .Select(amenityRoom => amenityRoom.ToAmenityRoomResponse())
            .ToList();
        
        return amenityRoomsResponse;
    }
    
    public async Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId)
    {
        if (amenityRoomId == null) return null;
        
        var amenityRoom = await _amenityRoomRepository.GetAmenityRoomById(amenityRoomId.Value);
        if (amenityRoom == null) return null;

        return amenityRoom.ToAmenityRoomResponse();
    }

    public async Task<List<AmenityRoomResponse>> GetFilteredDeletedAmenityRooms(string? searchString, Guid? roomTypeId)
    {
        var deletedAmenityRoom = await _amenityRoomRepository
            .GetFilteredDeletedAmenityRooms(searchString, roomTypeId);
        
        var amenityRoomsResponse = deletedAmenityRoom
            .Select(ar => ar.ToAmenityRoomResponse())
            .ToList();
        return amenityRoomsResponse;
    }
}