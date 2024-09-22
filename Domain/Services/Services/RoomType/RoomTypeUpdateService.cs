using Domain.DTO.RoomType;
using Domain.Repositories.IRepository;
using Domain.Services.IServices.IRoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeUpdateService : IRoomTypeUpdateService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeUpdateService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }
    
    public async Task<RoomTypeResponse?> UpdateRoomType(RoomTypeUpdateRequest roomTypeUpdateRequest)
    {
        if (roomTypeUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(roomTypeUpdateRequest));
        }
        var existingRoomType = await _roomTypeRepository.GetRoomTypeById(roomTypeUpdateRequest.Id);
        if (existingRoomType == null)
        {
            throw new ArgumentException("Id room type does not exist");
        }

        if (existingRoomType.Deleted)
        {
            throw new InvalidOperationException("This room type already deleted, cannot update it.");
        }
        
        existingRoomType.Name = roomTypeUpdateRequest.Name;
        existingRoomType.Description = roomTypeUpdateRequest.Description;
        existingRoomType.MaximumOccupancy = roomTypeUpdateRequest.MaximumOccupancy;
        existingRoomType.Status = roomTypeUpdateRequest.Status;
        existingRoomType.ModifiedTime = roomTypeUpdateRequest.ModifiedTime;
        existingRoomType.ModifiedBy = roomTypeUpdateRequest.ModifiedBy;
        
        await _roomTypeRepository.UpdateRoomType(existingRoomType);
        
        return existingRoomType.ToRoomTypeResponse();
    }
}