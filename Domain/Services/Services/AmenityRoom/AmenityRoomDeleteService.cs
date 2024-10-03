using Domain.DTO.AmenityRoom;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenityRoom;

namespace Domain.Services.Services.AmenityRoom;

public class AmenityRoomDeleteService : IAmenityRoomDeleteService
{
    private readonly IAmenityRoomRepository _amenityRoomRepository;

    public AmenityRoomDeleteService(IAmenityRoomRepository amenityRoomRepository)
    {
        _amenityRoomRepository = amenityRoomRepository;
    }
    
    public async Task<AmenityRoomResponse?> DeleteAmenityRoom(AmenityRoomDeleteRequest amenityRoomDeleteRequest)
    {
        if (amenityRoomDeleteRequest is null)
            throw new ArgumentNullException(nameof(amenityRoomDeleteRequest));

        var existingAmenityRoom = await _amenityRoomRepository
            .GetAmenityRoomById(amenityRoomDeleteRequest.Id);

        if (existingAmenityRoom is null)
            throw new Exception("No amenity room found");

        existingAmenityRoom.Status = (EntityStatus.Deleted);
        existingAmenityRoom.Deleted = true;
        existingAmenityRoom.DeletedTime = amenityRoomDeleteRequest.DeletedTime;
        existingAmenityRoom.DeletedBy = amenityRoomDeleteRequest.DeletedBy;
        
        await _amenityRoomRepository.DeleteAmenityRoom(existingAmenityRoom);
        return existingAmenityRoom.ToAmenityRoomResponse();
    }
}