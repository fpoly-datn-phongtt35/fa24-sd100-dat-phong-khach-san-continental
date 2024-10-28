using Domain.DTO.Amenity;
using Domain.DTO.AmenityRoom;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenityRoom;

namespace Domain.Services.Services.AmenityRoom;

public class AmenityRoomAddService : IAmenityRoomAddService
{
    private readonly IAmenityRoomRepository _amenityRoomRepository;

    public AmenityRoomAddService(IAmenityRoomRepository amenityRoomRepository)
    {
        _amenityRoomRepository = amenityRoomRepository;
    }

    public async Task<AmenityRoomResponse> AddAmenityRoomService(AmenityRoomAddRequest amenityRoomAddRequest)
    {
        if (amenityRoomAddRequest is null)
        {
            throw new ArgumentNullException(nameof(amenityRoomAddRequest));
        }
        // convert amenityRoomAddRequest into AmenityRoom type
        var amenityRoom = amenityRoomAddRequest.ToAmenityRoom();
        
        amenityRoom.Deleted = false;
        amenityRoom.DeletedTime = default;
        amenityRoom.ModifiedTime = default;

        await _amenityRoomRepository.AddAmenityRoom(amenityRoom);

        return amenityRoom.ToAmenityRoomResponse();
    }
}