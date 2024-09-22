using Domain.DTO.RoomType;
using Domain.Enums;
using Domain.Repositories.IRepository;
using Domain.Repositories.Repository;
using Domain.Services.IServices.IRoomType;

namespace Domain.Services.Services.RoomType;

public class RoomTypeDeleteService : IRoomTypeDeleteService
{
    private readonly IRoomTypeRepository _roomTypeRepository;

    public RoomTypeDeleteService(IRoomTypeRepository roomTypeRepository)
    {
        _roomTypeRepository = roomTypeRepository;
    }
    
    public async Task<RoomTypeResponse?> DeleteRoomType(RoomTypeDeleteRequest roomTypeDeleteRequest)
    {
        if (roomTypeDeleteRequest is null)
        {
            throw new ArgumentNullException(nameof(roomTypeDeleteRequest));
        }
        var existingRoomType = await _roomTypeRepository.GetRoomTypeById(roomTypeDeleteRequest.Id);

        if (existingRoomType is null)
        {
            throw new Exception("Room Type not found");
        }

        existingRoomType.Status = (EntityStatus)3;
        existingRoomType.Deleted = true;
        existingRoomType.DeletedTime = roomTypeDeleteRequest.DeletedTime;
        existingRoomType.DeletedBy = roomTypeDeleteRequest.DeletedBy;
        
        await _roomTypeRepository.DeleteRoomType(existingRoomType);

        return existingRoomType.ToRoomTypeResponse();
    }
}