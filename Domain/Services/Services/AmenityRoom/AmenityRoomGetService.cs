using Domain.DTO.AmenityRoom;
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

    public async Task<List<AmenityRoomResponse>> GetAllAmenityRooms()
    {
        var amenityRooms = await _amenityRoomRepository.GetAllAmenityRooms();

        var amenityRoomsResponse = amenityRooms
            .Select(amenityRoom => amenityRoom.ToAmenityRoomResponse())
            .ToList();
        
        return amenityRoomsResponse;
    }

    public async Task<AmenityRoomResponse?> GetAmenityRoomById(Guid? amenityRoomId)
    {
        if (amenityRoomId == null) return null;
        
        var amenityRoom = await _amenityRoomRepository.GetAmenityRoomById(amenityRoomId.Value);
        if(amenityRoom == null) return null;

        return amenityRoom.ToAmenityRoomResponse();

    }
}