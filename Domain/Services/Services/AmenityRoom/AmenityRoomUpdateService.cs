using Domain.DTO.AmenityRoom;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IAmenityRoom;

namespace Domain.Services.Services.AmenityRoom;

public class AmenityRoomUpdateService : IAmenityRoomUpdateService
{
    private readonly IAmenityRoomRepository _amenityRoomRepository;

    public AmenityRoomUpdateService(IAmenityRoomRepository amenityRoomRepository)
    {
        _amenityRoomRepository = amenityRoomRepository;
    }
    
    public async Task<AmenityRoomResponse?> UpdateAmenityRoom(AmenityRoomUpdateRequest amenityRoomUpdateRequest)
    {
        if (amenityRoomUpdateRequest is null)
            throw new ArgumentNullException(nameof(amenityRoomUpdateRequest));

        var existingAmenityRoom = await _amenityRoomRepository
            .GetAmenityRoomById(amenityRoomUpdateRequest.Id);
        
        if (existingAmenityRoom is null)
            throw new Exception("Id amenity room does not exist");

        if ((bool)existingAmenityRoom.Deleted)
            throw new InvalidOperationException("This amenity room type already deleted, cannot update it.");
        
        existingAmenityRoom.AmenityId = amenityRoomUpdateRequest.AmenityId;
        existingAmenityRoom.RoomTypeId = amenityRoomUpdateRequest.RoomTypeId;
        existingAmenityRoom.Amount = amenityRoomUpdateRequest.Amount;
        existingAmenityRoom.Status = amenityRoomUpdateRequest.Status;
        existingAmenityRoom.ModifiedTime = amenityRoomUpdateRequest.ModifiedTime;
        existingAmenityRoom.ModifiedBy = amenityRoomUpdateRequest.ModifiedBy;
        
        await _amenityRoomRepository.UpdateAmenityRoom(existingAmenityRoom);
        return existingAmenityRoom.ToAmenityRoomResponse();
    }

    public async Task<AmenityRoomResponse?> RecoverDeletedAmenityRoom
        (AmenityRoomUpdateRequest amenityRoomUpdateRequest)
    {
        if(amenityRoomUpdateRequest is null)
            throw new ArgumentNullException(nameof(amenityRoomUpdateRequest));
        
        var existingAmenityRoom = await _amenityRoomRepository
            .GetAmenityRoomById(amenityRoomUpdateRequest.Id);
        if(existingAmenityRoom is null)
            throw new Exception("Id amenity room does not exist");
        
        if((bool)!existingAmenityRoom.Deleted)
            throw new Exception("This amenity room is not deleted, cannot recover it.");
        
        existingAmenityRoom.Status = EntityStatus.Active;
        existingAmenityRoom.ModifiedTime = amenityRoomUpdateRequest.ModifiedTime;
        existingAmenityRoom.ModifiedBy = amenityRoomUpdateRequest.ModifiedBy;
        existingAmenityRoom.Deleted = false;
        existingAmenityRoom.DeletedTime = default;
        existingAmenityRoom.DeletedBy = default;
        
        await _amenityRoomRepository.RecoverDeletedAmenityRoom(existingAmenityRoom);
        return existingAmenityRoom.ToAmenityRoomResponse();
    }
}